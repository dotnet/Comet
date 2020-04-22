using System;
using System.Collections.ObjectModel;

namespace System.Maui.Samples
{
	public class Issue123 : View
	{
		private readonly State<int> count = 0;

		[Body]
		View body() => new NavigationView{ new VStack
			{
				new Label(() => $"Value: {count.Value}")
					.Color(Color.Black)
					.FontSize(32),
				new Button("Increment", () => count.Value ++ )
					.Frame(width:320, height:44)
					.Background(Color.Black)
					.Color(Color.White)
					.Margin(20)
				,
			}
		};
	}
}
