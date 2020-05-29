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
		
		[Theory]
		[MemberData(nameof(GetAllTestData))]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "Makes the test look nice in Test Explorer")]
		public void TestCodeSample(string testName, string source, string jsonExpected)
		{
			var codeParser = new CometViewParser();
			var parse = codeParser.ParseCode(source).ToList();
			var text = JsonConvert.SerializeObject(parse);

			Assert.Equal(jsonExpected, text);
		}

	}
}
