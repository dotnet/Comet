using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;


namespace Comet.SourceGenerator
{
	[Generator]
	public class AutoNotifyGenerator : ISourceGenerator
	{
		private const string attributeText = @"
using System;
namespace Comet
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class AutoNotifyAttribute : Attribute
    {
        public AutoNotifyAttribute()
        {
        }
        public string PropertyName { get; set; }
    }
}
";

		public void Initialize(GeneratorInitializationContext context)
		{
			context.RegisterForPostInitialization((i) => i.AddSource("AutoNotifyAttribute", attributeText));
			// Register a syntax receiver that will be created for each generation pass
			context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
		}

		public void Execute(GeneratorExecutionContext context)
		{
			// add the attribute text

			// retreive the populated receiver 
			if (!(context.SyntaxContextReceiver is SyntaxReceiver receiver) || !receiver.Fields.Any())
				return;
			// we're going to create a new compilation that contains the attribute.
			// TODO: we should allow source generators to provide source during initialize, so that this step isn't required.
			CSharpParseOptions options = (context.Compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
			Compilation compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(attributeText, Encoding.UTF8), options));


			var attributeSymbol = context.Compilation.GetTypeByMetadataName("Comet.AutoNotifyAttribute");
			var notifySymbol = context.Compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");
			var notifyReadSymbol = compilation.GetTypeByMetadataName("Comet.INotifyPropertyRead");
			var autoImplementedSymbol = compilation.GetTypeByMetadataName("Comet.IAutoImplemented");

			// group the fields by class, and generate the source
			foreach (IGrouping<INamedTypeSymbol, IFieldSymbol> group in receiver.Fields.GroupBy(f => f.ContainingType))
			{
				string classSource = ProcessClass(group.Key, group.ToList(), attributeSymbol, notifySymbol, notifyReadSymbol, autoImplementedSymbol, context);
				if(!string.IsNullOrWhiteSpace(classSource))
					context.AddSource($"{group.Key.Name}_autoNotify.cs", SourceText.From(classSource, Encoding.UTF8));
			}

		
		
		}
		private string ProcessClass(INamedTypeSymbol classSymbol, List<IFieldSymbol> fields, ISymbol attributeSymbol, ISymbol notifySymbol, ISymbol notifyReadSymbol, ISymbol autoImplementedSymbol, GeneratorExecutionContext context)
		{
			if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
			{
				context.ReportDiagnostic(Diagnostic.Create("AutoGen101", "Compiler", message: $"{classSymbol.ToDisplayString()} cannot be a nested class in order to use the [AutoNotify] attribute.", DiagnosticSeverity.Error, defaultSeverity: DiagnosticSeverity.Error,true,0)) ;
				return null; //TODO: issue a diagnostic that it must be top level
			}

			string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

			// begin building the generated source
			StringBuilder source = new StringBuilder($@"
using Comet;
namespace {namespaceName}
{{
    public partial class {classSymbol.Name} : {notifyReadSymbol.ToDisplayString()} , {autoImplementedSymbol.ToDisplayString()}
    {{
");

			// if the class doesn't implement INotifyPropertyChanged already, add it
			if (!classSymbol.Interfaces.Contains(notifySymbol))
			{
				source.Append("public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;");
			}
			// if the class doesn't implement INotifyPropertyChanged already, add it
			if (!classSymbol.Interfaces.Contains(notifyReadSymbol))
			{
				source.Append("public event System.ComponentModel.PropertyChangedEventHandler PropertyRead;");
			}

			// create properties for each field 
			foreach (IFieldSymbol fieldSymbol in fields)
			{
				ProcessField(source, fieldSymbol, attributeSymbol);
			}

			source.Append("} }");
			return source.ToString();
		}

		private void ProcessField(StringBuilder source, IFieldSymbol fieldSymbol, ISymbol attributeSymbol)
		{
			// get the name and type of the field
			string fieldName = fieldSymbol.Name;
			ITypeSymbol fieldType = fieldSymbol.Type;

			// get the AutoNotify attribute from the field, and any associated data
			AttributeData attributeData = fieldSymbol.GetAttributes().Single(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
			TypedConstant overridenNameOpt = attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "PropertyName").Value;

			string propertyName = chooseName(fieldName, overridenNameOpt);
			if (propertyName.Length == 0 || propertyName == fieldName)
			{
				//TODO: issue a diagnostic that we can't process this field
				return;
			}

			source.Append($@"
public {fieldType} {propertyName} 
{{
    get 
    {{
		StateManager.OnPropertyRead(this, nameof({propertyName}));
		this.PropertyRead?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof({propertyName})));
        return this.{fieldName};
    }}
    set
    {{
        this.{fieldName} = value;
		StateManager.OnPropertyChanged(this, nameof({propertyName}), value);
        this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof({propertyName})));
    }}
}}
");

			string chooseName(string fieldName, TypedConstant overridenNameOpt)
			{
				if (!overridenNameOpt.IsNull)
				{
					return overridenNameOpt.Value.ToString();
				}

				fieldName = fieldName.TrimStart('_');
				if (fieldName.Length == 0)
					return string.Empty;

				if (fieldName.Length == 1)
					return fieldName.ToUpper();

				return fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1);
			}

		}

		/// <summary>
		/// Created on demand before each generation pass
		/// </summary>
		class SyntaxReceiver : ISyntaxContextReceiver
		{
			public List<IFieldSymbol> Fields { get; } = new List<IFieldSymbol>();

			/// <summary>
			/// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
			/// </summary>
			public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
			{
				// any field with at least one attribute is a candidate for property generation
				if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax
					&& fieldDeclarationSyntax.AttributeLists.Count > 0)
				{
					foreach (VariableDeclaratorSyntax variable in fieldDeclarationSyntax.Declaration.Variables)
					{
						// Get the symbol being declared by the field, and keep it if its annotated
						IFieldSymbol fieldSymbol = context.SemanticModel.GetDeclaredSymbol(variable) as IFieldSymbol;
						if (fieldSymbol.GetAttributes().Any(ad => ad.AttributeClass.ToDisplayString() == "Comet.AutoNotifyAttribute"))
						{
							Fields.Add(fieldSymbol);
						}
					}
				}
			}
		}
	}
}
