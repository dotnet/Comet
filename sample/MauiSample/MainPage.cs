using System;
using Comet;

namespace MauiSample
{
	public class MainPage : View
	{
		[Body]
		View body ()=> new VStack(spacing: 5)
		{
			new Text("This top part is a Xamarin.Platform.VerticalStackLayout"),
			new HStack(spacing:2)
			{
				new Button("A Button").Frame(width:100).Color(Comet.Color.White),
				new Button("Hello I'm a button")
					.Color(Comet.Color.Green)
					.Background(Comet.Color.Purple),
				new Text("And these buttons are in a HorizontalStackLayout"),
			},
			new Text("Hey"),
			new SecondView(),

		}.Background(Comet.Color.Beige);
	}

	
}
