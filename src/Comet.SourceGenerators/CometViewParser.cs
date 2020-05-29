using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Comet.SourceGenerators
{
	public class CometViewParser
	{
		public IEnumerable<ClassObject> ParseCode(string source)
		{

			var tree = CSharpSyntaxTree.ParseText(source);
			var root = tree.GetRoot() as CompilationUnitSyntax;
			var classes = root.Members.OfType<ClassDeclarationSyntax>();
			foreach (var cd in classes)
				yield return Parse(cd);
		}

		public ClassObject Parse(ClassDeclarationSyntax cd)
		{
			var result = new ClassObject
			{
				Name = cd.Identifier.ToString(),
			};
			var methods = cd.Members.OfType<MethodDeclarationSyntax>().ToList();//.Select(ParseMethods).Where(x => x != null).ToList();
			foreach (var method in methods)
			{
				var body = Parse(method);
				if (body != null)
					result.BodyCreation.AddRange(body);
			}
			return result;
		}

		public List<ObjectCreation> Parse(MethodDeclarationSyntax md)
		{
			if (!md.AttributeLists.OfType<AttributeListSyntax>()
				.Any(x => x.Attributes.FirstOrDefault(y => y.ToString() == "Body") != null))
			{
				return null;
			}
			//We found a method with  [Body] !!!!
			var foundItems = new List<IdentifierNameSyntax>();
			var body = md.Body?.Statements.Cast<SyntaxNode>().ToList() ?? new List<SyntaxNode> { md.ExpressionBody?.Expression };
			var objectCreations = new List<ObjectCreation>();

			foreach (var node in body)
			{
				ParseNode(node, objectCreations);
			}
			return objectCreations;

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
		ObjectCreation Parse(ObjectCreationExpressionSyntax node, List<ObjectCreation> objects)
		{
			var result = new ObjectCreation
			{
				Name = node.Type.ToString(),
			};

			if (node.ArgumentList?.Arguments.Count > 0)
			{
				foreach (var arg in node.ArgumentList?.Arguments)
				{
					var identifiers = FindIdentifierNameSyntax(arg).ToList();
					result.ObjectParameters.Add(
						new ObjectParameter
						{
							BoundProperties = identifiers.Select(x => x.ToString()).ToList()
						});
				}
			}
			if(node.Initializer != null)
			{
				foreach (var n in node.Initializer.Expressions)
					ParseNode(n, objects);
			}

			return result;
		}

		void ParseNode( SyntaxNode node, List<ObjectCreation> objects)
		{
			if (node is ObjectCreationExpressionSyntax oce)
			{
				var obj = Parse(oce, objects);
				if (obj != null)
				{
					objects.Add(obj);
				}
			}
			else if (node is ReturnStatementSyntax rss)
			{
				ParseNode(rss.Expression, objects);
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
