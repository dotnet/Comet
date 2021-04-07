using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Hosting;

namespace Comet.Samples
{
	public class MyApp : CometApp
	{
		readonly State<int> clickCount = 0;
		readonly State<double> progress = .5;
		[Body]
		View view() =>
			 
			new VStack(spacing: 20)
			{
				new Text("Welcome to Comet!!!").Margin(top: 100),
				new HStack()
				{
					new Text("Label 1"),
					new Text("Label 2"),
				},
				new Button(()=> $"I was Clicked: {clickCount}",()=>{
					clickCount.Value++;
				}).Color(Colors.Yellow)
					.Background(Colors.Blue),
				new ActivityIndicator(),
				new CheckBox(),
				new DatePicker(),
				new ProgressBar(progress),
				new Slider(progress),


			//}
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
