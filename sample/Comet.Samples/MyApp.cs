using System;
using System.Linq;
using Comet.Samples.Models;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Hosting;

namespace Comet.Samples
{
	public class MyApp : CometApp
	{
		readonly State<int> clickCount = 0;
		readonly State<double> progress = .5;
		readonly State<bool> isToggled = false;
		readonly State<string> textValue = "Test";
		readonly State<TimeSpan> timePickerTime = TimeSpan.FromSeconds(0);
		[Body]
		View view() =>
			 
			new VStack(spacing: 6)
			{
				new Text("Welcome to Comet!").Margin(top: 100).Color(Colors.Blue),
				// new Image("turtlerock.jpg").Frame(100,100), 
				new ShapeView(new Circle().Stroke(Colors.Fuchsia,2)).Frame(100,100).Background(Colors.Green),
				new Text(() => !isToggled ?  "Off" : "Hey I am toggled"),
				new Button(()=>  $"I was Clicked: {clickCount}!!!!!",()=>{
					clickCount.Value++;
				}).Color(Colors.Yellow)
					.Background(Colors.Blue),
				new ActivityIndicator(),
				new CheckBox(isToggled),
				new Toggle(isToggled),
				new TextEditor(textValue),
				new ProgressBar(progress),
				new Slider(progress),
				new Text(textValue),
				new TextField(textValue),
				new SecureField(textValue),
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
