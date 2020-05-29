using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Comet.SourceGenerators;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Comet.SourceGenSample
{
	class Program
	{
		static void Main(string[] args)
		{
			var code = File.ReadAllText("CodeSample1.cs");

			var syntaxTree = CSharpSyntaxTree.ParseText(code);

			var references = new List<MetadataReference>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in assemblies)
			{
				if (!assembly.IsDynamic)
				{
					references.Add(MetadataReference.CreateFromFile(assembly.Location));
				}
			}

			var compilation = CSharpCompilation.Create("foo", new SyntaxTree[] { syntaxTree }, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));


			var parser = new CometViewParser();
			var result = parser.ParseCode(compilation).ToList();
		}
	}
}
