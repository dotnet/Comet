using System;
using System.Collections.Generic;

namespace System.Maui.Samples
{
	public class TextFieldSample2 : View
	{
		readonly State<string> _textValue = "Edit Me";

		[Body]
		View body() => new VStack()
		{
			new Entry(_textValue, "Name"),
			new HStack()
			{
				new Label("Current Value:")
					.Color(Color.Grey),
				new Label(_textValue),
				new Spacer()
			},
		}.FillHorizontal();
	}

}
