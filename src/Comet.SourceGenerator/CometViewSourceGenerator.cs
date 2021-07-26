using Comet.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stubble.Core.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//[assembly: Comet(typeof(IButton), nameof(IButton.Text))]

namespace Comet.SourceGenerator
{
	[Generator]
	public class CometViewSourceGenerator : ISourceGenerator
	{
		private const string attributeSource = @"
  	[System.AttributeUsage(System.AttributeTargets.Assembly, AllowMultiple = true)]
	internal class CometGenerateAttribute : System.Attribute
	{
		public CometGenerateAttribute(System.Type classType, params string[] keyProperties)
		{
			ClassType = classType;
			KeyProperties = keyProperties;
		}
		public string ClassName { get; set; }
		public System.Type ClassType { get; }
		public string[] KeyProperties { get; }
		public string Namespace { get; set; }
		public System.Type BaseClass { get; set; }
		public string[] DefaultValues { get; set; }
	}
";


		const string classMustacheTemplate = @"
using System;
using Comet;
using Microsoft.Maui;
namespace {{NameSpace}} {
	public partial class {{ClassName}} : {{BaseClassName}} 
	{

		public {{ClassName}} ({{#ParametersFunction}} Binding<{{Type}}> {{LowercaseName}}{{DefaultValueString}}{{/ParametersFunction}})
		{
			{{#Parameters}}
			{{Name}} = {{LowercaseName}};
			{{/Parameters}}
		}

		{{#FuncConstructorFunction}}		
		public {{ClassName}} ({{#ParametersFunction}} Func<{{Type}}> {{LowercaseName}}{{DefaultValueString}}{{/ParametersFunction}})
		{
			{{#Parameters}}
			{{Name}} = {{LowercaseName}};
			{{/Parameters}}
		}
		{{/FuncConstructorFunction}}

		{{#Parameters}}
		Binding<{{Type}}> {{LowercaseName}};
		public Binding<{{Type}}> {{Name}}
		{
			get => {{LowercaseName}};
			private set => this.SetBindingValue(ref  {{LowercaseName}}, value);
		}
		{{/Parameters}}
		
		{{#Properties}}
		{{#PropertiesFunc}}
		{{/PropertiesFunc}}
		{{/Properties}}
	}

}
";

		const string extensionMustacheTemplate = @"
using Comet;
using Microsoft.Maui;
using System;
namespace {{NameSpace}} {
	public static partial class {{ClassName}}Extensions
	{		
		{{#Properties}}
		{{#ExtensionPropertiesFunc}}
		public static T {{Name}}<T>(this T view, Binding<{{Type}}> {{LowercaseName}}, bool cascades = true) where T : View =>
			view.SetEnvironment(nameof({{FullName}}),{{LowercaseName}},cascades) ?? {{DefaultValue}};
		
		public static T {{Name}}<T>(this T view, Func<{{Type}}> {{LowercaseName}}, bool cascades = true) where T : View =>
			view.SetEnvironment(nameof({{FullName}}),(Binding<{{Type}}>){{LowercaseName}},cascades) ?? {{DefaultValue}};

		{{/ExtensionPropertiesFunc}}
		{{/Properties}}
	
	}
}
";

		static Dictionary<(bool HasSet, bool IsMethod), (string FromEnvironment, string FromProperty)> interfacePropertyDictionary;

		static CometViewSourceGenerator()
		{
			var interfacePropertyEnvironmentMustache = @"
                {{Type}} {{FullName}} {
                        get => this.GetEnvironment<{{CleanType}}>(nameof({{FullName}})) ?? {{DefaultValue}};
                        set => this.SetEnvironment(nameof({{FullName}}), value);
                }
";

			var interfacePropertyGetOnlyEnvironmentMustache = @"
                {{Type}} {{FullName}} => this.GetEnvironment<{{CleanType}}>(nameof({{FullName}})) ?? {{DefaultValue}};
";

			var interfacePropertyMustache = @"
                {{Type}} {{FullName}} {
                        get => {{Name}} ?? {{DefaultValue}};
                        set => {{Name}}.Set(value);
                }
";

			var interfacePropertyGetOnlyMustache = @"
                {{Type}} {{FullName}} => {{Name}} ?? {{DefaultValue}};
";

			var interfacePropertyMethodEnvironmentMustache = @"
                void {{FullName}} () => this.GetEnvironment<{{CleanType}}>(nameof({{FullName}}))?.Invoke();
";

			var interfacePropertyMethodMustache = @"
                void {{FullName}} () => {{Name}}.CurrentValue?.Invoke();
";

			interfacePropertyDictionary = new Dictionary<(bool HasSet, bool IsMethod), (string FromEnvironment, string FromProperty)>
			{
				[(true, false)] = (interfacePropertyEnvironmentMustache, interfacePropertyMustache),
				[(false, false)] = (interfacePropertyGetOnlyEnvironmentMustache, interfacePropertyGetOnlyMustache),
				[(false, true)] = (interfacePropertyMethodEnvironmentMustache, interfacePropertyMethodMustache),
			};
		}

		Stubble.Core.StubbleVisitorRenderer stubble = new StubbleBuilder().Build();
		public void Execute(GeneratorExecutionContext context)
		{
			
			SyntaxReceiver rx = (SyntaxReceiver)context.SyntaxContextReceiver!;
			
			foreach (var item in rx.TemplateInfo)
			{
				var input = GetModelData(item.name, item.interfaceType, item.keyProperties, item.nameSpace, item.baseClass, item.propertyNameTransforms, item.propertyDefaultValues);
				var classSource = stubble.Render(classMustacheTemplate, input);
				
				context.AddSource($"{item.name}.g.cs", classSource);

				var extensionSource = stubble.Render(extensionMustacheTemplate, input);
				context.AddSource($"{item.name}Extension.g.cs", extensionSource);
			}
		}
		public static string GetFullName(ISymbol symbol, string ending = null)
		{
			//TODO: loop through and get parent
			var name = Combine(symbol.Name, ending);
			if (symbol.ContainingNamespace != null)
				return GetFullName(symbol.ContainingNamespace, name);

			return name;
		}
		static string Combine(string first, string second)
		{
			if (string.IsNullOrWhiteSpace(second))
				return first;
			if (string.IsNullOrWhiteSpace(first))
				return second;
			return $"{first}.{second}";
		}

		dynamic GetModelData(string name, INamedTypeSymbol interfaceType, List<string> keyProperties, string nameSpace, INamedTypeSymbol baseClass, Dictionary<string, string> propertyNameTransforms, Dictionary<string, string> propertyDefaultValues)
		{
			var interfaces = interfaceType.AllInterfaces.ToList();
			interfaces.Insert(0, interfaceType);
			var alreadyImplemented = baseClass.AllInterfaces;
			interfaces.RemoveAll(x => alreadyImplemented.Contains(x));
			List<(string Type, string CleanType, string Name, string FullName, bool IsMethod, bool ShouldBeExtension)> properties = new();
			Dictionary<string, string> constructorTypes = new Dictionary<string, string>();
			List<string> propertiesWithSetters = new();
			Dictionary<string, bool> quoteDefaultData = new();
			foreach(var i in interfaces)
			{
				var members = i.GetMembers();
				foreach(var m in members)
				{
					if (m.Name.StartsWith("get_"))
						continue;
					if (m.Name.StartsWith("set_"))
						propertiesWithSetters.Add(m.Name.Replace("set_", ""));

					string type = "";
					bool canBeNull = true;
					bool isMethod = false;
					if (m is IMethodSymbol mi)
					{
						type = typeof(Action).FullName;
						isMethod = true;
					}
					else if(m is IPropertySymbol pi)
					{
						//cleanType = GetFullName(pi.Type.WithNullableAnnotation(pi.Type.NullableAnnotation));
						canBeNull = !pi.Type.IsValueType;
						type = GetFullName(pi.Type);
						if (pi.Type.Name  == "String")
							quoteDefaultData[m.Name] = true;
					}

					var cleanType = canBeNull ? type : $"{type}?";

					if (keyProperties.Contains(m.Name))
					{
						constructorTypes[m.Name] = cleanType;
						properties.Add((type, cleanType, m.Name, $"{GetFullName(i)}.{m.Name}", isMethod, false));
					}
					else
					{
						properties.Add((type, cleanType, m.Name, $"{GetFullName(i)}.{m.Name}", isMethod, true));

					}
				}


			}
			List<(string Type, string Name, string defaultValueString)> constructorParameters = new();
			for(var i = 0; i < keyProperties.Count; i++)
			{
				var keyName = keyProperties[i];
				var value = constructorTypes[keyName];
				var defaultValue = i == 0 ? "" : " = null";
				constructorParameters.Add((value, keyName, defaultValue));
			}

			string getPropertyDefaultValue(string key) 
			{
				if (!propertyDefaultValues.TryGetValue(key, out var defaultValue))
					return "default";
				var value = quoteDefaultData.TryGetValue(key, out var shouldQuote) && shouldQuote ? $"\"{defaultValue}\"" : defaultValue;
				return $"{defaultValue}";
			};
			string getNewName(string key)
			{
				if (propertyNameTransforms.TryGetValue(key, out var defaultValue))
					return defaultValue;
				return key;
			};

			var input = new
			{
				ClassName = name,
				BaseClassName = $"{baseClass} , {string.Join(",", interfaces.Select(x => GetFullName(x)))}",
				NameSpace = nameSpace,
				Parameters = constructorParameters.Select(x => new
				{
					x.Type,
					Name = getNewName(x.Name),
					LowercaseName = getNewName(x.Name).LowercaseFirst(),
					DefaultValueString = x.defaultValueString
				}).ToList(),
				HasParameters = constructorParameters.Any(),
				Properties = properties.Select(x=> new
				{
					x.Type,
					x.CleanType,
					Name = getNewName(x.Name),
					x.FullName,
					x.IsMethod,
					HasSet = propertiesWithSetters.Contains(x.Name),
					x.ShouldBeExtension,
					ClassName = name,
					DefaultValue = getPropertyDefaultValue(x.Name),
					LowercaseName = x.Name.LowercaseFirst(),
				}).ToList(),
				ParametersFunction = new Func<dynamic, string, object>((dyn, str) => string.Join(",", ((IEnumerable<dynamic>)dyn.Parameters).Select(x => stubble.Render(str, new
				{
					x.Type,
					x.Name,
					x.LowercaseName,
					x.DefaultValueString
				})))),
				FuncConstructorFunction = new Func<dynamic, string, object>((dyn, str) => dyn.HasParameters ? stubble.Render(str, dyn) : ""),
				PropertiesFunc = new Func<dynamic, string, object>((dyn, str) => {
					var templateGroup = interfacePropertyDictionary[(dyn.HasSet, dyn.IsMethod)];
					var template = dyn.ShouldBeExtension ? templateGroup.FromEnvironment : templateGroup.FromProperty;
					return stubble.Render(template, dyn);
				}),
				ExtensionPropertiesFunc = new Func<dynamic, string, object>((dyn, str) => dyn.ShouldBeExtension ? stubble.Render(str, dyn) : ""),


			};
			return input;
		}

		public void Initialize(GeneratorInitializationContext context)
		{
  //if (!Debugger.IsAttached)
  //{
  //  Debugger.Launch();
  //}

			context.RegisterForPostInitialization((pi) => pi.AddSource("CometGenerationAttribute__", attributeSource));
			context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
		}

		class SyntaxReceiver : ISyntaxContextReceiver
		{
			public List<(string name, INamedTypeSymbol interfaceType, List<string> keyProperties, string nameSpace, INamedTypeSymbol baseClass, Dictionary<string,string> propertyNameTransforms, Dictionary<string, string> propertyDefaultValues)> TemplateInfo = new ();

			public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
			{
				var cometView = context.SemanticModel.Compilation.GetTypeByMetadataName("Comet.View");
				var attrib = context.Node as AttributeSyntax;
				if (attrib != null)
				{
					var type = context.SemanticModel.GetTypeInfo(attrib);
					if (context.SemanticModel.GetTypeInfo(attrib).Type?.ToDisplayString() == "CometGenerateAttribute")
					{
						{

							var f = attrib.ArgumentList.Arguments[0].Expression as TypeOfExpressionSyntax;
							var realClass = GetType(context, f);

							var keyProperties = new List<string>();
							string name = null;
							string nameSpace = null;
							INamedTypeSymbol baseClass = cometView;
							List<string> defaultValues = new();

							foreach (var arg in attrib.ArgumentList.Arguments.Skip(1))
							{
								var constVal = context.SemanticModel.GetConstantValue(arg.Expression);
								var symbol = context.SemanticModel.GetSymbolInfo(arg.Expression);
								var argName = arg.NameEquals?.Name.Identifier.ValueText;
								if(argName == "ClassName")
								{
									name = constVal.ToString();
									continue;
								}

								if(argName == "Namespace")
								{
									nameSpace = constVal.ToString();
									continue;
								}
								if(argName == "DefaultValues")
								{
									var iac = (arg.Expression as ImplicitArrayCreationExpressionSyntax).Initializer.Expressions;
									foreach(var i in iac)
									{
										var fif = context.SemanticModel.GetConstantValue(i);
										defaultValues.Add(fif.ToString());
									}
								}
								//Strings which can only be key properties
								if (arg.Expression is LiteralExpressionSyntax || (arg.Expression is InvocationExpressionSyntax && constVal.HasValue))
								{
									keyProperties.Add(constVal.ToString());
									continue;
								}
								

								if (arg.Expression is TypeOfExpressionSyntax toe)
								{
									//this is our base class!
									baseClass = GetType(context, toe);
									continue;
								}

								var exp = arg.Expression;
							}
							name ??= realClass.Name?.TrimStart('I');
							nameSpace ??= GetFullName(realClass.ContainingNamespace);

							Dictionary<string, string> propertyTransform = new();
							Dictionary<string, string> propertyDefaultValues = new();
							(bool hasParts,string key) getParts(string oldKey) {
								var hasName = oldKey.Contains(':');
								var hasValue = oldKey.Contains('=');

								if (!hasName && !hasValue)
								{
									return (false, null);
								}
								var parts = oldKey.Split(':', '=');
								var key = parts[0];
								var newName = hasName ? parts[1] : null;
								var defaultValue = hasValue ? parts.Last() : null;


								if (newName != null)
									propertyTransform[key] = newName;

								if (defaultValue != null)
									propertyDefaultValues[key] = defaultValue;
								return (true, key);
							}
							foreach(var oldKey in keyProperties.ToList())
							{
								(bool hasParts, string key) = getParts(oldKey);
								if(!hasParts)
								{
									continue;
								}
								var oldIndex = keyProperties.IndexOf(oldKey);
								keyProperties.RemoveAt(oldIndex);
								keyProperties.Insert(oldIndex, key);
							}


							foreach (var oldKey in defaultValues)
							{
								(bool hasParts, string key) = getParts(oldKey);
							}


							//string name = context.SemanticModel.GetTypeInfo(attrib.ArgumentList.Arguments[0].Expression).ToString();
							//string template = context.SemanticModel.GetConstantValue(attrib.ArgumentList.Arguments[1].Expression).ToString();
							//string hash = context.SemanticModel.GetConstantValue(attrib.ArgumentList.Arguments[2].Expression).ToString();

							TemplateInfo.Add((name,realClass,keyProperties,nameSpace,baseClass, propertyTransform, propertyDefaultValues));
						}
					}
				}
			}

			static INamedTypeSymbol GetType(GeneratorSyntaxContext context, TypeOfExpressionSyntax expression)
			{
				var interfaceType = context.SemanticModel.GetSymbolInfo(expression.Type);
				var s = GetFullName(interfaceType.Symbol);
				return context.SemanticModel.Compilation.GetTypeByMetadataName(s);
			}

		}
	}
}
