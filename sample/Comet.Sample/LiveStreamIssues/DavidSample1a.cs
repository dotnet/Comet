

using Microsoft.Maui.Graphics;

namespace Comet.Samples.LiveStreamIssues
{
	public class DavidSample1a : View
	{
		[Body]
		View body() =>
			new VStack(LayoutAlignment.Center)
			{
				new ShapeView(new Circle().Stroke(Colors.Black, 2f)).Frame(44,44)
			};
	}
}
