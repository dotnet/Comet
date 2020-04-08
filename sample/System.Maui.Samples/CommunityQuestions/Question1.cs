using System;

namespace System.Maui.Samples
{
	public class Question1 : View
	{
		[Body]
		View body()
		{
			return new ScrollView {
					new VStack {
						new Image("turtlerock.jpg").Frame(75, 75).Padding(4),
						new Text("Title"),
						new Text("Description").FontSize(12).Color(Color.Grey),
					}.FillHorizontal()

			};
		}
	}
}
