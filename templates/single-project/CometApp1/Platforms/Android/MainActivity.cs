using Android.App;
using Android.Content.PM;
using Microsoft.Maui;

namespace CometApp1
{

	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	[IntentFilter(
		new[] { Microsoft.Maui.Essentials.Platform.Intent.ActionAppAction },
		Categories = new[] { global::Android.Content.Intent.CategoryDefault })]
	public class MainActivity : MauiAppCompatActivity
	{
	}
}