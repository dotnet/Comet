using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.Samples
{
	public class SkiaSample3 : View
	{
		readonly State<double> _strokeSize = 2;
		readonly State<Color> _strokeColor = Colors.Black;

		[Body]
		View body() => 
			new VStack()
			{
				new HStack()
				{
					new Text("Stroke Width:"),
					new Slider(_strokeSize, 1, 10).FillHorizontal()
				}.FillHorizontal(),
				
					new Text("Stroke Color!:").HorizontalTextAlignment(TextAlignment.Center),
                //new ScrollView{
                    new HStack
					{
						new Button("Black", () =>
						{
							_strokeColor.Value = Colors.Black;
						}).Color(Colors.Black),
						new Button("Blue", () =>
						{
							_strokeColor.Value = Colors.Blue;
						}).Color(Colors.Blue),
						new Button("Red", () =>
						{
							_strokeColor.Value = Colors.Red;
						}).Color(Colors.Red),
					}.Alignment(Alignment.Center),
                //},
                new BindableFingerPaint(
					strokeSize:_strokeSize,
					strokeColor:_strokeColor).Frame(width:400, height:400).RoundedBorder(color:Colors.White)
		};
	}
}
