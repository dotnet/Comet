using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Comet.Samples;

namespace Comet.Android.Sample
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : CometActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
#if DEBUG
            Comet.Reload.Init();
#endif
            Comet.Skia.Android.UI.Init();
            Comet.Android.UI.Init();
            Registrar.Handlers.Register<Button, Skia.Android.SkiaControlHandler<Skia.Button>>();

            Page = new MainPage();
        }
    }
}

