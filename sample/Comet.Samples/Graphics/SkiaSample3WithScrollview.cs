//using System;
//using System.Collections.Generic;
//using System.Text;
//using Comet.Skia;

//namespace Comet.Samples.Skia
//{
//	public class SkiaSample3WithScrollView : View
//	{
//		readonly State<float> _strokeSize = 2;
//		readonly State<string> _strokeColor = "#000000";

//		[Body]
//		View body() => new VStack()
//		{
//			new VStack()
//			{
//				new HStack()
//				{
//					new Text("Stroke Width:"),
//					new Slider(_strokeSize, 1, 10, 1)
//				},
//				new HStack()
//				{
//					new Text("Stroke Color!:"),
//					new TextField(_strokeColor),
//				},
//				new ScrollView(Orientation.Horizontal)
//				{
//					new HStack(spacing:8)
//					{
//						new Button("Black", () =>
//						{
//							_strokeColor.Value = Colors.Black.ToHexString();
//						}),
//						new Button("Blue", () =>
//						{
//							_strokeColor.Value = Colors.Blue.ToHexString();
//						}),
//						new Button("Red", () =>
//						{
//							_strokeColor.Value = Colors.Red.ToHexString();
//						}),
//						new Button("Green", () =>
//						{
//							_strokeColor.Value = Colors.Green.ToHexString();
//						}),
//						new Button("Orange", () =>
//						{
//							_strokeColor.Value = Color.Orange.ToHexString();
//						}),
//						new Button("Yellow", () =>
//						{
//							_strokeColor.Value = Color.Yellow.ToHexString();
//						}),
//						new Button("Brown", () =>
//						{
//							_strokeColor.Value = Color.Brown.ToHexString();
//						}),
//						new Button("Salmon", () =>
//						{
//							_strokeColor.Value = Colors.Salmon.ToHexString();
//						}),
//						new Button("Magenta", () =>
//						{
//							_strokeColor.Value = Color.Magenta.ToHexString();
//						})
//					},
//				},
//				new BindableFingerPaint(
//					strokeSize:_strokeSize,
//					strokeColor:_strokeColor).ToView().Frame(height:400)
//			},
//		};
//	}
//}
