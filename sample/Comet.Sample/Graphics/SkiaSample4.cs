using System;
using System.Collections.Generic;
using System.Text;
namespace Comet.Samples
{
	public class SkiaSample4 : View
	{
		readonly State<double> _strokeSize = 2;
		readonly State<Color> _strokeColor = Colors.White ;

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
						new Text("Stroke Width:"),
						new Slider(_strokeSize, 1, 10).FillHorizontal()
					},
					new HStack()
					{
						new Text("Stroke Color:"),
						new TextField(new Binding<string>(() => _strokeColor.Value.ToArgbHex(),(s) => _strokeColor.Value = Color.FromArgb(s)))
					},
					new Button("Reset", () => fingerPaint.Reset()),
					fingerPaint.Frame(height: 400)
				},
			};
		}
	}
}
