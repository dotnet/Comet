using Foundation;
using Microsoft.Maui;
using Comet.Samples;
using Microsoft.Maui.Hosting;

namespace Maui.Controls.Sample.SingleProject
{
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate
	{
		protected override MauiApp CreateMauiApp() => MyApp.CreateMauiApp();
	}
}