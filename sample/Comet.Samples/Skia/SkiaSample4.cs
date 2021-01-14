//using System;
//using System.Collections.Generic;
//using System.Text;
//using Comet.Skia;

//namespace Comet.Samples.Skia
//{
//	public class SkiaSample4 : View
//	{
//		readonly State<float> _strokeSize = 2;
//		readonly State<string> _strokeColor = "#000000";

//		[Body]
//		View body()
//		{
//			var fingerPaint = new BindableFingerPaint(
//				strokeSize: _strokeSize,
//				strokeColor: _strokeColor);

//			return new VStack()
//			{
//				new VStack()
//				{
//					new HStack()
//					{
//						new Text("Stroke Width:"),
//						new Slider(_strokeSize, 1, 10, 1)
//					},
//					new HStack()
//					{
//						new Text("Stroke Color:"),
//						new TextField(_strokeColor)
//					},
//					new Button("Reset", () => fingerPaint.Reset()),
//					fingerPaint.ToView().Frame(height: 400)
//				},
//			};
//		}
//	}
//}
