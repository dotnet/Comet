using System;
using Android.App;
using Android.Runtime;
using Comet.Samples;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace CometSample
{
	[Application]
	public class MainApplication : MauiApplication
	{
		public MainApplication(IntPtr handle, JniHandleOwnership ownership)
			: base(handle, ownership)
		{
		}

		protected override MauiApp CreateMauiApp() => MyApp.CreateMauiApp();
	}
}