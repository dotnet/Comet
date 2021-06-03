using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Comet.Android;

namespace NewApp.Droid
{
    [Activity(Label = "NewApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : CometActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            #if (kind == "shell")
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            #endif
            #if (IncludeXamarinEssentials)
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            #endif
#if DEBUG
            Comet.Reload.Init();
#endif

            Page = new MainPage();
        }
        #if(IncludeXamarinEssentials)
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        #endif
    }
}