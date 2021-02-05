using System;
using System.Collections.Generic;

namespace Comet.Samples
{
	public class VStackSample : View
	{
		readonly State<string> _textValue = "Edit Me";
		readonly State<double> _sliderValue = 50;

		[Body]
		View body() => new VStack()
		{
			new Text(_textValue),
			new TextField(_textValue, "Name"),
			new SecureField(_textValue, "Name"),
			new Slider(_sliderValue),
			new ProgressBar(_sliderValue)
		}.FillHorizontal();
	}

}
