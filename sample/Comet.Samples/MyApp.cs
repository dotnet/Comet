using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Hosting;

namespace Comet.Samples
{
	public class MyApp : CometApp
	{
		[Body]
		View view() => new VStack(spacing: 20)
			{
				new Text("Hey!!").Margin(top: 100),
				//new Text("Hey!!"),
				//new Text("TEST PADDING!!!").Frame(height:30).Margin(top:100),
				new Text("This top part is a Microsoft.Maui.VerticalStackLayout"),
				new HStack(spacing:2)
				{
					new Button("A Button").Frame(width:100).Color(Colors.White),
					new Button("Hello I'm a button")
						.Color(Colors.Green)
						.Background(Colors.Purple),
					new Text("And these buttons are in a HorizontalStackLayout"),
				},
				new Text("Hey!!"),
				new Text("Hey!!"),
				//new SecondView(),

			}.Background(Colors.Beige);

		public override void Configure(IAppHostBuilder appBuilder)
		{
			base.Configure(appBuilder);

			appBuilder.UseMauiApp<MyApp>();
#if DEBUG
			appBuilder.EnableHotReload();
#endif

		}
	}
}
