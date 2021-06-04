using System;
using System.Linq;
using Comet.Graphics;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Hosting;

namespace CometApp1
{
	public class App : CometApp
	{
		readonly State<int> clickCount = 0;
		readonly State<double> progress = .5;
		readonly State<bool> isToggled = false;
		readonly State<string> textValue = "Test";
		readonly State<TimeSpan> timePickerTime = TimeSpan.FromSeconds(0);
		[Body]
		View view() => new MainPage();

		public override void Configure(IAppHostBuilder appBuilder)
		{
			base.Configure(appBuilder);

			appBuilder.UseMauiApp<App>();
#if DEBUG
			appBuilder.EnableHotReload();
#endif

		}
	}
}
