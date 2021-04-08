using Android.App;
using Microsoft.Maui;

namespace Comet.Android.Sample
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	[IntentFilter(
		new[] { Microsoft.Maui.Essentials.Platform.Intent.ActionAppAction },
		Categories = new[] { global::Android.Content.Intent.CategoryDefault })]
	public class MainActivity : MauiAppCompatActivity
	{	}
}

