

using Microsoft.Maui.Graphics;

namespace Comet.Samples.LiveStreamIssues
{
	public class DavidSample2 : View
	{
		[Body]
		View body() =>
			new VStack()
			{
				new HStack
				{
					new ShapeView(new Circle().Stroke(Colors.Black, 2f)).Frame(44,44)
				}
			}.Alignment(Alignment.BottomTrailing);
	}
}
