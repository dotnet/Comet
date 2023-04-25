using System;
using System.Collections.Generic;
using Comet.Styles;
namespace Comet.Samples
{
	public class DemoButtonStyle : ButtonStyle
	{
		public DemoButtonStyle()
		{
			TextColor = Colors.Red;
			BackgroundColor = Colors.White;
			Border = new RoundedRectangle(5).Stroke(Colors.Red, 1.5f, true);
			Shadow = new Graphics.Shadow().WithColor(Colors.White).WithRadius(1).WithOffset(new Point(1, 1));
		}
	}
	public class ButtonSample1 : View
	{
		readonly State<int> count = 0;

		[Body]
		View body() => new VStack
		{
			new Button("Increment Value", () => count.Value ++ )
				.Frame(200,40)
				.HorizontalLayoutAlignment(LayoutAlignment.Center)
				.VerticalLayoutAlignment(LayoutAlignment.Center)
				.Apply<DemoButtonStyle>(),
			new Text(() => $"Value: {count.Value}"),
		};

	}
}
