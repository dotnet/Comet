using System;

namespace System.Maui.Samples
{
	public class Question1c : View
	{
		[Body]
		View body() =>
			new VStack {
				new Image("turtlerock.jpg")
					.Frame(75, 75)
					.Padding(4),
				new Label("Title")
					.FitHorizontal(),
				new Label("Description")
					.FitHorizontal()
					.FontSize(12)
					.Color(Color.Grey),
			}.FillHorizontal();
	}
}