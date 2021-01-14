using System.Graphics;

namespace Comet.Samples.LiveStreamIssues
{
	public class DavidSample1 : View
	{
		[Body]
		View body() =>
			new VStack(HorizontalAlignment.Center)
			{
				new HStack
				{
					new ShapeView(new Circle().Stroke(Colors.Black, 2f)).Frame(44,44)
				}
			}.Frame(alignment: Alignment.Top);
	}
}
