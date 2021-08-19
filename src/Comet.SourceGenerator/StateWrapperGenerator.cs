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
  	[System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = true)]
	internal class GenerateStateClassAttribute : System.Attribute
	{
		public GenerateStateClassAttribute(System.Type classType)
		{
			ClassType = classType;
		}
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
	public partial class {{ClassName}} : IAutoImplemented 
	{

		public readonly {{ClassType}} OriginalModel;
		public {{ClassName}} ({{ClassType}} model)
		{
			OriginalModel = model;
			if (model is INotifyPropertyChanged inp)
			{
				inp.PropertyChanged += Inp_PropertyChanged;
				shouldNotifyChanged = false;
			}
		}

		bool shouldNotifyChanged = true;
		private void Inp_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			StateManager.OnPropertyChanged(sender,e.PropertyName,null);
		}
		void NotifyPropertychanged(object value, [CallerMemberName] string memberName = null)
		{
			if (shouldNotifyChanged)
				StateManager.OnPropertyChanged(this, memberName, value);
		}

		void NotifyPropertyRead([CallerMemberName] string memberName = null) => StateManager.OnPropertyRead(this, memberName);

		{{#Properties}}
		{{#PropertiesFunc}}
		{{/PropertiesFunc}}
		{{/Properties}}
	}
}
";

		static StateWrapperGenerator()
		{
			var interfacePropertyMustache = @"
        {{{Type}}} {{Name}} {
			get {
				NotifyPropertyRead();
				return OriginalModel.{{Name}};
			}
            set {
				OriginalModel.{{Name}} = value;
				NotifyPropertychanged(value);
			}
        }
";
			var interfacePropertySetOnlyMustache = @"
        {{{Type}}} {{Name}} {
			set {
				OriginalModel.{{Name}} = value;
				NotifyPropertychanged(value);
			}
        }
";

			var interfacePropertyGetOnlyMustache = @"
        {{{Type}}} {{Name}} {
			get {
				NotifyPropertyRead();
				return OriginalModel.{{Name}};
			}
        }
";
			interfacePropertyDictionary = new()
			{
				[(true,true)] = interfacePropertyMustache,
				[(true,false)] = interfacePropertyGetOnlyMustache,
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
				foreach(var baseType in GetAllBaseTypes(classType.BaseType))
					yield return baseType;
			}
		}

		dynamic GetModelData(string className, INamedTypeSymbol classType, string nameSpace)
		{

			var baseClasses = GetAllBaseTypes(classType).ToList();
			List<(string Type, string Name)> properties = new();
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
			

			var input = new {
				ClassName = className,
				ClassType = CometViewSourceGenerator.GetFullName(classType),
				NameSpace = nameSpace,
				Properties = properties.Select(x => new {
					Type = x.Type,
					Name = x.Name,
					HasSet = propertiesWithSetters.Contains(x.Name),
					HasGet = propertiesWithGetters.Contains(x.Name),
				}).ToList(),
				PropertiesFunc = new Func<dynamic, string, object>((dyn, str) => {
					var template = interfacePropertyDictionary[(dyn.HasGet, dyn.HasSet)];
					return stubble.Render(template, dyn);
				}),
			};
			return input;
		}

		public void Initialize(GeneratorInitializationContext context)
		{
			// if (!Debugger.IsAttached)
			// {
			// 	Debugger.Launch();
			// }

			context.RegisterForPostInitialization((pi) => pi.AddSource("GenerateStateClassAttribute__", attributeSource));
			context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
		}

		class SyntaxReceiver : ISyntaxContextReceiver
		{
			public List<(string name, INamedTypeSymbol classType, string nameSpace)> TemplateInfo = new();

			public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
			{
				var attrib = context.Node as AttributeSyntax;
				if (attrib != null)
				{
					var type = context.SemanticModel.GetTypeInfo(attrib);
					if (context.SemanticModel.GetTypeInfo(attrib).Type?.ToDisplayString() == "GenerateStateClassAttribute")
					{
						{

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


								var exp = arg.Expression;
							}
							name ??= $"{realClass.Name}State";
							nameSpace ??= CometViewSourceGenerator.GetFullName(realClass.ContainingNamespace);

						

							TemplateInfo.Add((name, realClass, nameSpace));
						}
					}
				}
			}

			static INamedTypeSymbol GetType(GeneratorSyntaxContext context, TypeOfExpressionSyntax expression)
			{
				var interfaceType = context.SemanticModel.GetSymbolInfo(expression.Type);
				var s = CometViewSourceGenerator.GetFullName(interfaceType.Symbol);
				return context.SemanticModel.Compilation.GetTypeByMetadataName(s);
			}

		}
	}
}
