using System;

namespace System.Maui.Samples
{
	public class Question1a : View
	{
		[Body]
		View body() =>
			new VStack {
						new Image("turtlerock.jpg").Frame(75, 75).Padding(4),
						new Label("Title"),
						new Label("Description").FontSize(12).Color(Color.Grey),
					}.FillHorizontal();
	}
}
