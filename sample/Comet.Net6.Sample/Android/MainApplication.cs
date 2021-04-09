using System;
using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Comet.Samples;

namespace HelloMaui
{
	[Application]
	public class MainApplication : MauiApplication<MyApp>
	{
		public MainApplication(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
		{
		}
	}
}