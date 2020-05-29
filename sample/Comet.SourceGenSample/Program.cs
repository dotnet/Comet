using System;
using System.IO;
using System.Linq;
using Comet.SourceGenerators;
namespace Comet.SourceGenSample
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			var parser = new CometViewParser();
			var code = File.ReadAllText("CodeSample1.cs");
			var result = parser.ParseCode(code).ToList() ;
		}
	}
}
