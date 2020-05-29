using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Comet.SourceGenerators
{
	public class CometViewParser
	{
		INamedTypeSymbol _cometView;
		INamedTypeSymbol _iNotifyPropertyChanged;

		public IEnumerable<ClassObject> ParseCode(Compilation compilation)
		{
			_cometView = compilation.GetTypeByMetadataName("Comet.View");
			_iNotifyPropertyChanged = compilation.GetTypeByMetadataName(typeof(INotifyPropertyChanged).FullName);

			foreach (var tree in compilation.SyntaxTrees)
			{
				var model = compilation.GetSemanticModel(tree);
				var root = tree.GetRoot() as CompilationUnitSyntax;
				var classes = root.Members.OfType<ClassDeclarationSyntax>();
				foreach (var cd in classes)
					yield return Parse(cd, model);
			}
		}

		public ClassObject Parse(ClassDeclarationSyntax cd, SemanticModel model)
		{
			var result = new ClassObject
			{
				Name = cd.Identifier.ToString(),
			};
			var methods = cd.Members.OfType<MethodDeclarationSyntax>().ToList();//.Select(ParseMethods).Where(x => x != null).ToList();
			foreach (var method in methods)
			{
				Parse(method, model, result);
			}

			var constructors = cd.Members.OfType<ConstructorDeclarationSyntax>().ToList();//.Select(ParseMethods).Where(x => x != null).ToList();
			foreach (var constructor in constructors)
			{
				Parse(constructor, model, result);
			}

			return result;
		}

		public void Parse(ConstructorDeclarationSyntax md, SemanticModel model, ClassObject classObject)
		{
			var body = md.Body?.Statements.Cast<SyntaxNode>().ToList() ?? new List<SyntaxNode> { md.ExpressionBody?.Expression };

			foreach (var node in body)
			{
				var assignment = FindAssignment(node);
				if (assignment != null && assignment.Left is IdentifierNameSyntax id)
				{
					var symbol = model.GetSymbolInfo(id);

					if (symbol.Symbol.Name == "Body" &&
						SymbolEqualityComparer.Default.Equals(symbol.Symbol.ContainingType, _cometView))
					{
						ParseNode(assignment.Right, model, classObject);
					}
				}
			}

			AssignmentExpressionSyntax FindAssignment(SyntaxNode node) => node switch
			{
				ExpressionStatementSyntax ess => FindAssignment(ess.Expression),
				AssignmentExpressionSyntax assignment => assignment,
				_ => null
			};
		}

		public void Parse(MethodDeclarationSyntax md, SemanticModel model, ClassObject classObject)
		{
			if (!md.AttributeLists.OfType<AttributeListSyntax>()
				.Any(x => x.Attributes.FirstOrDefault(y => y.ToString() == "Body") != null))
			{
				return;
			}
			//We found a method with  [Body] !!!!
			var foundItems = new List<IdentifierNameSyntax>();
			var body = md.Body?.Statements.Cast<SyntaxNode>().ToList() ?? new List<SyntaxNode> { md.ExpressionBody?.Expression };
			var objectCreations = new List<ObjectCreation>();

			foreach (var node in body)
			{
				ParseNode(node, model, classObject);
			}

		}

		public class ClassObject
		{
			public string Name { get; set; }
			public List<string> GlobalBoundItems { get; set; } = new List<string>();
			public List<ObjectCreation> BodyCreation { get; set; } = new List<ObjectCreation>();
		}

		public class ObjectCreation
		{
			public string Name { get; set; }
			public List<string> BoundProperties { get; set; } = new List<string>();
			public List<ObjectParameter> ObjectParameters { get; set; } = new List<ObjectParameter>();
			public string Id { get; set; }

		}
		public class ObjectParameter
		{
			public List<string> BoundProperties { get; set; } = new List<string>();
		}
		ObjectCreation Parse(ObjectCreationExpressionSyntax node, SemanticModel model, ClassObject classObject)
		{
			var symbol = model.GetSymbolInfo(node.Type);
			if (!(symbol.Symbol is INamedTypeSymbol typeSymbol) || !RoslynHelpers.IsDerivedFrom(typeSymbol, _cometView))
			{
				return null;
			}

			var result = new ObjectCreation
			{
				Name = node.Type.ToString(),
			};

			if (node.ArgumentList?.Arguments.Count > 0)
			{
				foreach (var arg in node.ArgumentList?.Arguments)
				{
					var identifiers = FindIdentifierNameSyntax(arg).ToList();

					List<string> boundProperties = new List<string>();
					foreach (var id in identifiers)
					{
						symbol = model.GetSymbolInfo(id);
						var type = symbol.Symbol switch
						{
							IFieldSymbol field => field.Type,
							IPropertySymbol prop => prop.Type,
							_ => null
						};
						if (type != null && RoslynHelpers.ImplementsInterface(type, _iNotifyPropertyChanged))
						{
							boundProperties.Add(id.Identifier.Text);
						}
					}
					result.ObjectParameters.Add(
						new ObjectParameter
						{
							BoundProperties = boundProperties
						});
				}
			}
			if (node.Initializer != null)
			{
				foreach (var n in node.Initializer.Expressions)
					ParseNode(n, model, classObject);
			}

			return result;
		}

		void ParseNode(SyntaxNode node, SemanticModel model, ClassObject classObject)
		{
			if (node is ObjectCreationExpressionSyntax oce)
			{
				var obj = Parse(oce, model, classObject);
				if (obj != null)
				{
					classObject.BodyCreation.Add(obj);
				}
			}
			else if (node is ReturnStatementSyntax rss)
			{
				ParseNode(rss.Expression, model, classObject);
			}
			else if (node is CastExpressionSyntax ces)
			{
				ParseNode(ces.Expression, model, classObject);
			}
			else if (node is ConditionalExpressionSyntax conditional)
			{
				classObject.GlobalBoundItems.AddRange(FindIdentifierNameSyntax(conditional.Condition).Select(x => x.Identifier.ToString()));
				ParseNode(conditional.WhenTrue, model, classObject);
				ParseNode(conditional.WhenFalse, model, classObject);
			}
			else if (node is ParenthesizedExpressionSyntax pes)
			{
				ParseNode(pes.Expression, model, classObject);
			}
			else if (node is ParenthesizedLambdaExpressionSyntax ples)
			{
				ParseNode(ples.Body, model, classObject);
			}
			else
			{
				Console.WriteLine(node.GetType());
			}
		}

		IEnumerable<IdentifierNameSyntax> FindIdentifierNameSyntax(SyntaxNode node)
		{
			if (node is IdentifierNameSyntax ins)
				yield return ins;
			foreach (var c in node.ChildNodes().SelectMany(FindIdentifierNameSyntax))
				yield return c;
		}
	}
}
