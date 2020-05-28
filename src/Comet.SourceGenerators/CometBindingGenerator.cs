using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Comet.Generators
{
	[Generator]
	public class CometBindingGenerator : ISourceGenerator
	{
		private const string attributeText = @"
using System;
namespace AutoNotify
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

		public void Initialize(InitializationContext context)
		{
			//Debugger.Launch();
			// Register a syntax receiver that will be created for each generation pass
			context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
		}

		public void Execute(SourceGeneratorContext context)
		{

			// retreive the populated receiver 
			if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
				return;

			// we're going to create a new compilation that contains the attribute.
			// TODO: we should allow source generators to provide source during initialize, so that this step isn't required.
			CSharpParseOptions options = (context.Compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
			Compilation compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(attributeText, Encoding.UTF8), options));

			// get the newly bound attribute, and INotifyPropertyChanged
			//INamedTypeSymbol attributeSymbol = compilation.GetTypeByMetadataName("AutoNotify.AutoNotifyAttribute");
			//INamedTypeSymbol notifySymbol = compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");
			//var notifyReadSymbol = compilation.GetTypeByMetadataName("Comet.INotifyPropertyRead");
			//var autoImplementedSymbol = compilation.GetTypeByMetadataName("Comet.IAutoImplemented");
			// loop over the candidate fields, and keep the ones that are actually annotated
			List<IFieldSymbol> fieldSymbols = new List<IFieldSymbol>();
			//foreach (FieldDeclarationSyntax field in receiver.CandidateFields)
			//{
			//	SemanticModel model = compilation.GetSemanticModel(field.SyntaxTree);
			//	foreach (VariableDeclaratorSyntax variable in field.Declaration.Variables)
			//	{
			//		// Get the symbol being decleared by the field, and keep it if its annotated
			//		IFieldSymbol fieldSymbol = model.GetDeclaredSymbol(variable) as IFieldSymbol;
			//		if (fieldSymbol.GetAttributes().Any(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default)))
			//		{
			//			fieldSymbols.Add(fieldSymbol);
			//		}
			//	}
			//}

			// group the fields by class, and generate the source
			//foreach (IGrouping<INamedTypeSymbol, IFieldSymbol> group in fieldSymbols.GroupBy(f => f.ContainingType))
			//{
			//	string classSource = ProcessClass(group.Key, group.ToList(), attributeSymbol, notifySymbol, notifyReadSymbol, autoImplementedSymbol, context);
			//	context.AddSource($"{group.Key.Name}_autoNotify.cs", SourceText.From(classSource, Encoding.UTF8));
			//}
		}

		private string ProcessClass(INamedTypeSymbol classSymbol, List<IFieldSymbol> fields, ISymbol attributeSymbol, ISymbol notifySymbol, ISymbol notifyReadSymbol, ISymbol autoImplementedSymbol, SourceGeneratorContext context)
		{
			if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
			{
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

		class ClassObject
		{
			public ClassDeclarationSyntax Node { get; set; }
			public string Name { get; set; }

			public bool IsView { get; set; }

			public List<CurrentObject> Methods { get; } = new List<CurrentObject>();
		}

		class CurrentObject
		{
			public string Name { get; set; }
			public SyntaxNode Node { get; set; }

			public List<IdentifierNameSyntax> UsedNames { get; } = new List<IdentifierNameSyntax>();


			public List<CurrentObject> Children { get; } = new List<CurrentObject>();
		}

		/// <summary>
		/// Created on demand before each generation pass
		/// </summary>
		class SyntaxReceiver : ISyntaxReceiver
		{


			public List<FieldDeclarationSyntax> CandidateFields { get; } = new List<FieldDeclarationSyntax>();
			public List<IdentifierNameSyntax> UsedNames = new List<IdentifierNameSyntax>();
			/// <summary>
			/// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
			/// </summary>
			/// 
			public List<ClassObject> Views = new List<ClassObject>();

			ClassObject currentClass;
			CurrentObject currentObject;
			public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
			{
				//Find all ClassDeclarationSyntax that subclass View
				if (syntaxNode is ClassDeclarationSyntax cd)
				{
					var c = ParseThePuppy(cd);
					if (c.IsView)
						Views.Add(c);
					//if (currentClass?.IsView ?? false)
					//{
					//	if (currentObject != null)
					//		currentClass.Methods.Add(currentObject);
					//	currentObject = null;
					//	Views.Add(currentClass);
					//}
					//currentClass = new ClassObject
					//{
					//	Node = cd,
					//	IsView = cd.BaseList?.Types.OfType<SimpleBaseTypeSyntax>().Any(x => x.ToString() == "View") ??false,
					//};
					//var bases = cd.BaseList?.Types.OfType<SimpleBaseTypeSyntax>().ToList();
					//var baseNames = bases.Select(x => x.ToString()).ToList();
				}

				////Find all MethodDeclarationSyntax that have an Attribute
				////We are looking for the [Body]

				//if(syntaxNode is MethodDeclarationSyntax md)
				//{

				//}

				////Check if it's an ExpressionBody


				////
				//if(syntaxNode is IdentifierNameSyntax id)
				//{
				//	UsedNames.Add(id);
				//}

				//// any field with at least one attribute is a candidate for property generation
				//if (syntaxNode is FieldDeclarationSyntax fieldDeclarationSyntax
				//                && fieldDeclarationSyntax.AttributeLists.Count > 0)
				//            {
				//                CandidateFields.Add(fieldDeclarationSyntax);
				//            }

				//ClassDeclarationSyntax
			}

			ClassObject ParseThePuppy(ClassDeclarationSyntax cd)
			{
				var c = new ClassObject
				{
					Node = cd,
					IsView = cd.BaseList?.Types.OfType<SimpleBaseTypeSyntax>().Any(x => x.ToString() == "View") ?? false,
				};

				var methods = cd.Members.OfType<MethodDeclarationSyntax>().Select(ParseMethods).Where(x => x != null).ToList();
				return c;
			}

			CurrentObject ParseMethods(MethodDeclarationSyntax md)
			{
				if (!md.AttributeLists.OfType<AttributeListSyntax>().Any(x => x.Attributes.FirstOrDefault(y => y.ToString() == "Body") != null))
				{
					return null;
				}
				var co = new CurrentObject
				{
					Name = md.Identifier.Text,
				};
				//now we loop through each sntax treee
				//We collect each IdentifierNameSyntax And add them to a list
				//We find all ObjectInitializers
				//Clear the list and add them to globals, at the end of the object Initlizers we then 


				return co;
			}
		}
	}
}
