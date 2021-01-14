using System;
using System.Collections.Generic;
using System.Graphics;

namespace Comet.Samples
{
	public class TextFieldSample2 : View
	{
		readonly State<string> _textValue = "Edit Me";

		[Body]
		View body() => new VStack()
		{
			new TextField(_textValue, "Name"),
			new HStack()
			{
				new Text("Current Value:")
					.Color(Colors.Grey),
				new Text(_textValue),
				new Spacer()
			},
		}.FillHorizontal();
	}

}
