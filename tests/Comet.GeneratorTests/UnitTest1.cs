using System;
using System.IO;
using System.Linq;
using Comet.SourceGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;
using Xunit;
using Path = System.IO.Path;

namespace GeneratorTests {
	public class UnitTest1 :BaseTest {
		[Fact]
		public void CodeSample1Test() => TestCodeSample("CodeSample1");

		[Fact]
		public void CodeSample2Test() => TestCodeSample("CodeSample2");
		

		void TestCodeSample(string fileName)
		{
			var codeParser = new CometViewParser();
			var data = GetTestData(fileName);
			var parse = codeParser.ParseCode(data.Source).ToList();
			var text = JsonConvert.SerializeObject(parse);

			Assert.Equal(data.JsonExpectedResult, text);
		}

	}
}
