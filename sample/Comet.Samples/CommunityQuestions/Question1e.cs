using System;

namespace Comet.Samples
{
	public class Question1e : View
	{
		[Body]
		View body()
		{
			return new ScrollView {
					new VStack {
						new Image("turtlerock.jpg").Frame(75, 75).Padding(4),
						new Text("Title").TextAlignment(TextAlignment.Center),
						new Text("Description").TextAlignment(TextAlignment.Center).FontSize(12).Color(Color.Grey),
					}.FillHorizontal()

			};
		}
	}
}
