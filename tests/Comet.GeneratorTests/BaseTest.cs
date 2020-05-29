using System;
using System.IO;

namespace GeneratorTests
{
	public class BaseTest
	{
		public static (string Source, string JsonExpectedResult) GetTestData(string file) =>
			(File.ReadAllText(Path.Combine("CodeParsingSamples", $"{file}.cs")),
			File.ReadAllText(Path.Combine("CodeParsingSamples", $"{file}.txt")));
	
		public BaseTest()
		{
		}
	}
}
