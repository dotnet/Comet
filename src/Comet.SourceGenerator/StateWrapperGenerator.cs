using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stubble.Core.Builders;

namespace Comet.SourceGenerator
{
	[Generator]
	public class StateWrapperGenerator : ISourceGenerator
	{
		private const string attributeSource = @"
  	[System.AttributeUsage(System.AttributeTargets.Assembly | System.AttributeTargets.Class, AllowMultiple = true)]
	public class GenerateStateClassAttribute : System.Attribute
	{
		public GenerateStateClassAttribute(){}
		public GenerateStateClassAttribute(System.Type classType) => ClassType = classType;
		public string ClassName { get; set; }
		public System.Type ClassType { get; }
		public string Namespace { get; set; }
	}
";

		const string classMustacheTemplate = @"
using System;
using Comet;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace {{NameSpace}} {
	public partial class {{ClassName}} :INotifyPropertyRead, IAutoImplemented 
	{
		public event PropertyChangedEventHandler PropertyRead;
		public event PropertyChangedEventHandler PropertyChanged;

		public readonly {{ClassType}} OriginalModel;

		bool shouldNotifyChanged = true;

		public {{ClassName}} ({{ClassType}} model)
		{
			OriginalModel = model;
			if (model is INotifyPropertyChanged inpc)
			{
				inpc.PropertyChanged += Inpc_PropertyChanged;
				shouldNotifyChanged = false;
			}
		}

		void Inpc_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			StateManager.OnPropertyChanged(sender, e.PropertyName, null);
			PropertyChanged?.Invoke(sender, e);
		}
	
		void NotifyPropertyChanged(object value, [CallerMemberName] string memberName = null){
			if (shouldNotifyChanged) {
				StateManager.OnPropertyChanged(this, memberName, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
			}
		}
		
		void NotifyPropertyRead([CallerMemberName] string memberName = null){ 
			InitDirtyProperty(memberName);
			StateManager.OnPropertyRead(this, memberName);
			PropertyRead?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
		
		/// <summary>
		/// Notifies Comet of all changes in the underlying model (OriginalModel) observed properties 
		/// </summary>
		public void NotifyChanged(){
			if (shouldNotifyChanged){
				{{#Properties}}
				{{#PropertiesUpdateAllFunc}}
				{{/PropertiesUpdateAllFunc}}
				{{/Properties}}
			}
		}

		void InitDirtyProperty([CallerMemberName] string memberName = null){
			switch (memberName){
				{{#Properties}}
				{{#PropertiesInitFunc}}
				{{/PropertiesInitFunc}}
				{{/Properties}}
			}
		}

		void UpdateDirtyProperty([CallerMemberName] string memberName = null){
			switch (memberName){
				{{#Properties}}
				{{#PropertiesUpdateFunc}}
				{{/PropertiesUpdateFunc}}
				{{/Properties}}
			}
		}
		{{#Properties}}
		{{#PropertiesFunc}}
		{{/PropertiesFunc}}
		{{/Properties}}
	}
}
";

		static StateWrapperGenerator()
		{

			var interfacePropSupportMustache = @"
		bool {{LName}}IsObserved = false;
		bool {{LName}}IsDirty => {{LName}}IsObserved && !OriginalModel.{{Name}}.Equals({{LName}}LastValue);
		{{{Type}}} {{LName}}LastValue;
";

			var interfacePropertyMustache = interfacePropSupportMustache + 
@"		public {{{Type}}} {{Name}} {
			get {
				NotifyPropertyRead();
				return OriginalModel.{{Name}};
			}
			set {
				OriginalModel.{{Name}} = value;
				{{LName}}LastValue = value;
				NotifyPropertyChanged(value);
			}
		}
";
			var interfacePropertySetOnlyMustache = interfacePropSupportMustache + 
@"		public {{{Type}}} {{Name}} {
			set {
				OriginalModel.{{Name}} = value;
				{{LName}}LastValue = value;
				NotifyPropertyChanged(value);
			}
		}
";

			var interfacePropertyGetOnlyMustache = interfacePropSupportMustache + 
@"		public {{{Type}}} {{Name}} {
			get {
				return OriginalModel.{{Name}};
			}
		}
";
			interfacePropertyDictionary = new()
			{
				[(true, true)] = interfacePropertyMustache,
				[(true, false)] = interfacePropertyGetOnlyMustache,
				[(false, true)] = interfacePropertySetOnlyMustache,

			};
		}

		Stubble.Core.StubbleVisitorRenderer stubble = new StubbleBuilder().Build();
		static Dictionary<(bool HasGet, bool HasSet), string> interfacePropertyDictionary;
		public void Execute(GeneratorExecutionContext context)
		{
			if (!(context.SyntaxContextReceiver is SyntaxReceiver rx) || !rx.TemplateInfo.Any())
				return;

			foreach (var item in rx.TemplateInfo)
			{
				var input = GetModelData(item.name, item.classType, item.nameSpace);
				var classSource = stubble.Render(classMustacheTemplate, input);

				context.AddSource($"{item.name}.g.cs", classSource);
			}
		}

		IEnumerable<INamedTypeSymbol> GetAllBaseTypes(INamedTypeSymbol classType)
		{
			yield return classType;
			if (classType.BaseType != null)
			{
				foreach (var baseType in GetAllBaseTypes(classType.BaseType))
					yield return baseType;
			}
		}

		dynamic GetModelData(string className, INamedTypeSymbol classType, string nameSpace)
		{
			var baseClasses = GetAllBaseTypes(classType).ToList();
			List<(string Type, string Name)> properties = new();
			List<(string Type, string Name)> methods = new();

			List<string> propertiesWithSetters = new();
			List<string> propertiesWithGetters = new();

			foreach (var i in baseClasses)
			{
				var members = i.GetMembers();
				foreach (var m in members)
				{
					var name = m.Name;
					if (m.Name.StartsWith("get_"))
					{
						name = name.Replace("get_", "");
						propertiesWithGetters.Add(name);
						continue;
					}
					else if (m.Name.StartsWith("set_"))
					{
						name = name.Replace("set_", "");
						propertiesWithSetters.Add(name);
						continue;
					}
					//else if(m is IMethodSymbol mi && mi.DeclaredAccessibility == Accessibility.Public)
					//{
					//	var x = 0;
					//}

					string type = null;
					List<(string Type, string Name)> parameters = new List<(string Type, string Name)>();
					if (m is IPropertySymbol pi)
					{
						type = CometViewSourceGenerator.GetFullName(pi.Type);
						var t = (type, name);
						if (!properties.Contains(t))
							properties.Add(t);
					}
				}

			}

			var interfacePropInitMustache = 
@"				case ""{{Name}}"":
					if (!{{LName}}IsObserved){
						{{LName}}LastValue = OriginalModel.{{Name}};
						{{LName}}IsObserved = true;
					}
				break;
";

			var interfacePropUpdateMustache = 
@"				case ""{{Name}}"": 
					if ({{LName}}IsDirty){
						{{LName}}LastValue = OriginalModel.{{Name}};
						NotifyPropertyChanged({{LName}}LastValue, memberName);
					}
				break;
";

			var interfacePropUpdateAllMustache = 
@"				if ({{LName}}IsObserved){
					UpdateDirtyProperty(""{{Name}}"");
				}
";

			var input = new {
				ClassName = className,
				ClassType = CometViewSourceGenerator.GetFullName(classType),
				NameSpace = nameSpace,
				Properties = properties.Select(x => new {
					Type = x.Type,
					Name = x.Name,
					LName = x.Name.LowercaseFirst(),
					HasSet = propertiesWithSetters.Contains(x.Name),
					HasGet = propertiesWithGetters.Contains(x.Name),
				}).ToList(),
				PropertiesFunc = new Func<dynamic, string, object>((dyn, str) => {
					var template = interfacePropertyDictionary[(dyn.HasGet, dyn.HasSet)];
					return stubble.Render(template, dyn);
				}),
				PropertiesUpdateFunc = new Func<dynamic, string, object>((dyn, str) => {
					var template = dyn.HasGet ? interfacePropUpdateMustache : "";
					return stubble.Render(template, dyn);
				}),
				PropertiesUpdateAllFunc = new Func<dynamic, string, object>((dyn, str) => {
					var template = dyn.HasGet ? interfacePropUpdateAllMustache : "";
					return stubble.Render(template, dyn);
				}),
				PropertiesInitFunc = new Func<dynamic, string, object>((dyn, str) => {
					var template = dyn.HasGet ? interfacePropInitMustache : "";
					return stubble.Render(template, dyn);
				})

			};
			return input;
		}

		public void Initialize(GeneratorInitializationContext context)
		{
			//if (!Debugger.IsAttached)
			//{
			//	Debugger.Launch();
			//}

			context.RegisterForPostInitialization((pi) => pi.AddSource("GenerateStateClassAttribute__", attributeSource));
			context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
		}
		//[assembly: GenerateStateClass(typeof(GTApp.POCOPlain))]

		class SyntaxReceiver : ISyntaxContextReceiver
		{
			public List<(string name, INamedTypeSymbol classType, string nameSpace)> TemplateInfo = new();

			public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
			{
				if (context.Node is ClassDeclarationSyntax cds && context.SemanticModel.GetDeclaredSymbol(cds).GetAttributes().Any(x => x.AttributeClass.Name == "GenerateStateClassAttribute"))
				{
					var realClass = context.SemanticModel.GetDeclaredSymbol(cds).OriginalDefinition as INamedTypeSymbol;//GetType(context, f);
					string name = null;
					string nameSpace = null;
					name ??= $"{realClass.Name}State";
					nameSpace ??= CometViewSourceGenerator.GetFullName(realClass.ContainingNamespace);
					TemplateInfo.Add((name, realClass, nameSpace));


					//List<SyntaxNode> childClassProps = cds.ChildNodes().Where(x => x is PropertyDeclarationSyntax pds && pds.Type.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.ClassDeclaration).Select(X => X).ToList();
					// do
					// {
					// 	childClassProps = cds.ChildNodes().Where(x=>x is PropertyDeclarationSyntax pds)

					// } while (childClassProps.Any());
				}

				if (context.Node is AttributeSyntax attrib)
				{
					if (context.SemanticModel.GetTypeInfo(attrib).Type?.ToDisplayString() == "GenerateStateClassAttribute")
					{
						if (attrib.ArgumentList == null) return;

						var f = attrib.ArgumentList.Arguments[0].Expression as TypeOfExpressionSyntax;
						var realClass = GetType(context, f);

						string name = null;
						string nameSpace = null;

						foreach (var arg in attrib.ArgumentList.Arguments.Skip(1))
						{
							var constVal = context.SemanticModel.GetConstantValue(arg.Expression);
							var symbol = context.SemanticModel.GetSymbolInfo(arg.Expression);
							var argName = arg.NameEquals?.Name.Identifier.ValueText;
							if (argName == "ClassName")
							{
								name = constVal.ToString();
								continue;
							}

							if (argName == "Namespace")
							{
								nameSpace = constVal.ToString();
								continue;
							}
						}
						name ??= $"{realClass.Name}State";
						nameSpace ??= CometViewSourceGenerator.GetFullName(realClass.ContainingNamespace);
						TemplateInfo.Add((name, realClass, nameSpace));

					}
				}
			}

			//	static 

			static INamedTypeSymbol GetType(GeneratorSyntaxContext context, TypeOfExpressionSyntax expression)
			{
				var interfaceType = context.SemanticModel.GetSymbolInfo(expression.Type);
				var s = CometViewSourceGenerator.GetFullName(interfaceType.Symbol);
				return context.SemanticModel.Compilation.GetTypeByMetadataName(s);
			}

		}
	}
}
