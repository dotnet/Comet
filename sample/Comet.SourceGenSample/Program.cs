using System;
using AutoNotify;
namespace Comet.SourceGenSample
{
	//public partial class MyModel 
	//{  
	//	[AutoNotify]
	//	string foo;

	//	[AutoNotify]
	 
	//	//[AutoNotify]
	//	//bool heyGuys;

	//} 

	public class MyView : View
	{ 
		State<int> count = 0; 
		State<bool> shouldCelebrate = false; 

		[Environment]
		public readonly string Foo;

		[Body]
		View body() => new VStack
		{
			//(shouldCelebrate ?(View)new Text("Celebrate!!!!") : new Spacer()),
			new Text(()=>$"{count} times"),
			new Button("Click Me!", ()=> count.Value++),
		};

		public MyView(object MyModel)
		{
			Body = () => new Text(MyModel.ToString());
		}
		void
	}

	class Program
	{
		static void Main(string[] args)
		{
			//var model = new MyModel();
			//var view = new View
			//{
			//	Body = () => new Text(model.Foo),
			//};
			//Console.WriteLine("Hello World!");
		}
	}
}
