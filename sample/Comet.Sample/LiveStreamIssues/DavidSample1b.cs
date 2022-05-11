

using Microsoft.Maui.Graphics;

namespace Comet.Samples.LiveStreamIssues
{
	public class DavidSample1b : View
	{
		[Body]
		View body() =>
			new HStack(LayoutAlignment.Center)
			{
				new Spacer(),
				new ShapeView(new Circle().Stroke(Colors.Black, 2f)).Frame(44,44),
				new Spacer()
			};
	}
}
