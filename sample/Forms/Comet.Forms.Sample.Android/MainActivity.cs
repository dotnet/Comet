using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace System.Maui.Forms.Sample.Droid {
	[Activity (Label = "System.Maui.Forms.Sample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
		protected override void OnCreate (Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate (savedInstanceState);

			Xamarin.Essentials.Platform.Init (this, savedInstanceState);
			global::Xamarin.Forms.Forms.Init (this, savedInstanceState);
			LoadApplication (new System.Maui.Forms.Sample.App());
		}
		public override void OnRequestPermissionsResult (int requestCode, string [] permissions, [GeneratedEnum] Android.Content.PM.Permission [] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult (requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult (requestCode, permissions, grantResults);
		}
	}
}
