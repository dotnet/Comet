using System;
using Comet;

namespace MauiSample
{
	public class SecondView : View
	{
		[Body]
		View body() => new HStack{
			new Text("This is from the second view!!!!"),
			//new Text("I should get deleted"),

		};
	}
}
