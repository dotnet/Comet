using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Comet.Samples;

namespace CometSample
{
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate
	{
#pragma warning disable CA1416 // Validate platform compatibility
		protected override MauiApp CreateMauiApp() => MyApp.CreateMauiApp();
#pragma warning restore CA1416 // Validate platform compatibility
	}
}