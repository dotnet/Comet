using System;
using System.Collections.Generic;

namespace System.Maui.Samples
{
	public class TextFieldSample1 : View
	{
		readonly State<string> name1 = "";
		readonly State<string> name2 = "";

		[Body]
		View body() => new VStack()
		{
			new Entry(null, "Name", name1, name2),
			new HStack()
			{
				new Label("onEditingChanged:"),
				new Label(name1),
				new Spacer()
			},
			new HStack()
			{
				new Label("onCommit:"),
				new Label(name2),
				new Spacer()
			},
		}.FillHorizontal();
	}

}
