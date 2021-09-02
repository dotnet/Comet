using Foundation;
using Microsoft.Maui;
using Comet.Samples;

namespace Maui.Controls.Sample.SingleProject
{
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate
	{
		protected override MauiApp CreateMauiApp() => MyApp.CreateMauiApp();
	}
}