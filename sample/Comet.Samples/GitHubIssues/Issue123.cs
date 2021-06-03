using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;

namespace Comet.Samples
{
	public class Issue123 : View
	{
		private readonly State<int> count = 0;

		[Body]
		View body() => new NavigationView{ new VStack
			{
				new Text(() => $"Value: {count.Value}")
					.Color(Colors.Black)
					.FontSize(32),
				new Button("Increment", () => count.Value ++ )
					.Frame(width:320, height:44)
					.Background(Colors.Black)
					.Color(Colors.White)
					.Margin(20)
				,
			}
		};
	}
}
