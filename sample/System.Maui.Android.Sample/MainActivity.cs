using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Maui.Samples;

namespace System.Maui.Android.Sample
{
	[Activity(Label = "@string/app_name", MainLauncher = true)]
	public class MainActivity : MauiActivity
	{

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
#if DEBUG
			Comet.Reload.Init();
#endif
			System.Maui.Skia.UI.Init();

			//Replaces native controls with Skia drawn controls
			//System.Maui.Skia.Controls.Init ();

			Page = new MainPage();
		}
	}
}

