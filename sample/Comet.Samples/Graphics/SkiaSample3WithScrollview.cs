using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.Samples
{
	public class SkiaSample3WithScrollView : View
	{
		readonly State<double> _strokeSize = 2;
		readonly State<Color> _strokeColor = Colors.Black;

		[Body]
		View body() => new VStack()
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
					new Text("Stroke Color!:"),
				},
				new ScrollView(Orientation.Horizontal)
				{
					new HStack(spacing:8)
					{
						new Button("Black", () =>
						{
							_strokeColor.Value = Colors.Black;
						}),
						new Button("Blue", () =>
						{
							_strokeColor.Value = Colors.Blue;
						}),
						new Button("Red", () =>
						{
							_strokeColor.Value = Colors.Red;
						}),
						new Button("Green", () =>
						{
							_strokeColor.Value = Colors.Green;
						}),
						new Button("Orange", () =>
						{
							_strokeColor.Value = Colors.Orange;
						}),
						new Button("Yellow", () =>
						{
							_strokeColor.Value = Colors.Yellow;
						}),
						new Button("Brown", () =>
						{
							_strokeColor.Value = Colors.Brown;
						}),
						new Button("Salmon", () =>
						{
							_strokeColor.Value = Colors.Salmon;
						}),
						new Button("Magenta", () =>
						{
							_strokeColor.Value = Colors.Magenta;
						})
					},
				},
				new BindableFingerPaint(
					strokeSize:_strokeSize,
					strokeColor:_strokeColor).Frame(height:400).FillHorizontal().Border(new Rectangle().Stroke(Colors.White,2))
			},
		};
	}
}
