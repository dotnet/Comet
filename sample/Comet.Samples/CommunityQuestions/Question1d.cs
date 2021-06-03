using System;
using Microsoft.Maui.Graphics;

namespace Comet.Samples
{
	public class Question1d : View
	{
		[Body]
		View body()
		{
			return new ScrollView {
					new VStack {
						new Image("turtlerock.jpg").Frame(75, 75).Padding(4),
						new Text("Title").FitHorizontal(),
						new Text("Description").FitHorizontal().FontSize(12).Color(Colors.Grey),
					}.FillHorizontal()

			};
		}
	}
}
