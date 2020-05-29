using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace GeneratorTests
{
	public class BaseTest
	{
		public static IEnumerable<object[]> GetAllTestData()
		{
			foreach (var file in Directory.EnumerateFiles("CodeParsingSamples"))
			{
				string name = Path.GetFileNameWithoutExtension(file);
				var data = GetTestData(name);
				yield return new object[] { name, data.Source, data.JsonExpectedResult };
			}
		}

		public static (string Source, string JsonExpectedResult) GetTestData(string file) =>
			(File.ReadAllText(Path.Combine("CodeParsingSamples", $"{file}.cs")),
			File.ReadAllText(Path.Combine("CodeParsingSamples", $"{file}.txt")));

	}
}
