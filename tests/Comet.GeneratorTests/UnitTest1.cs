using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Comet;
using Comet.SourceGenerators;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Newtonsoft.Json;

using Xunit;

using Path = System.IO.Path;

namespace GeneratorTests
{
	public class UnitTest1 : BaseTest
	{

		[Theory]
		[MemberData(nameof(GetAllTestData))]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "Makes the test look nice in Test Explorer")]
		public void TestCodeSample(string testName, string source, string jsonExpected)
		{
			var codeParser = new CometViewParser();

			var syntaxTree = CSharpSyntaxTree.ParseText(source);

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

			System.Collections.Immutable.ImmutableArray<Diagnostic> diagnostics = compilation.GetDiagnostics();
			Assert.False(diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error), "Failed: " + diagnostics.FirstOrDefault()?.GetMessage());

			var parse = codeParser.ParseCode(compilation).ToList();
			var text = JsonConvert.SerializeObject(parse);

			Assert.Equal(jsonExpected, text);
		}

	}
}
