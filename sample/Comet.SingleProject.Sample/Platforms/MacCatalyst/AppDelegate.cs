using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Comet.Samples;

namespace CometSample
{
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate
	{
		protected override MauiApp CreateMauiApp() => MyApp.CreateMauiApp();
	}
}