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
		public void Test1 ()
		{
			var codeParser = new CometViewParser();
			var data = GetTestData("CodeSample1");
			var parse = codeParser.ParseCode(data.Source).ToList();
			var text = JsonConvert.SerializeObject(parse);

			Assert.Equal(data.JsonExpectedResult, text);
		}
	}
}
