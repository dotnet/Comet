using System.Graphics;

namespace Comet.Samples.LiveStreamIssues
{
	public class DavidSample1c : View
	{
		[Body]
		View body() =>
			new VStack(HorizontalAlignment.Center)
			{
				new HStack
				{
					new ShapeView(new Circle().Stroke(Colors.Black, 2f))
						.Frame(44,44)
				}
			};
	}
}
