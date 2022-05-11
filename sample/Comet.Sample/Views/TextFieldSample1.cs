using System;
using System.Collections.Generic;

namespace Comet.Samples
{
	public class TextFieldSample1 : View
	{
		readonly State<string> name1 = "";

		[Body]
		View body() => new VStack()
		{
			new TextField(name1, "Name", ()=>{
				Console.WriteLine("Completed");
			}),
			
			new HStack()
			{
				new Text("onCommit:"),
				new Text(name1),
				new Spacer()
			},
		}.FillHorizontal();
	}

}
