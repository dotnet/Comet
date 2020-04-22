using System;

namespace System.Maui.Samples
{
	public class Question1e : View
	{
		[Body]
		View body()
		{
			return new ScrollView {
					new VStack {
						new Image("turtlerock.jpg").Frame(75, 75).Padding(4),
						new Label("Title").TextAlignment(TextAlignment.Center),
						new Label("Description").TextAlignment(TextAlignment.Center).FontSize(12).Color(Color.Grey),
					}.FillHorizontal()

			};
		}
	}
}
