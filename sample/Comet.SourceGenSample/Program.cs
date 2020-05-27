using System;
using AutoNotify;
namespace Comet.SourceGenSample
{
	public partial class MyModel
	{
		[AutoNotify]
		string foo;

		[AutoNotify]
		bool bar;

		//[AutoNotify]
		//bool heyGuys;

	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
		}
	}
}
