using System;
using System.Collections.Generic;
using System.Text;
using System.Maui.Skia;

namespace System.Maui.Samples.Skia
{
	public class SkiaSample4 : View
	{
		readonly State<float> _strokeSize = 2;
		readonly State<string> _strokeColor = "#000000";

		[Body]
		View body()
		{
			var fingerPaint = new BindableFingerPaint(
				strokeSize: _strokeSize,
				strokeColor: _strokeColor);

			return new VStack()
			{
				new VStack()
				{
					new HStack()
					{
						new Label("Stroke Width:"),
						new Slider(_strokeSize, 1, 10, 1)
					},
					new HStack()
					{
						new Label("Stroke Color:"),
						new Entry(_strokeColor)
					},
					new Button("Reset", () => fingerPaint.Reset()),
					fingerPaint.ToView().Frame(height: 400)
				},
			};
		}
	}
}
