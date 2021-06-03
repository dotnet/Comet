using System;
using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Comet.Samples;

namespace Comet.Android.Sample
{
	[Application]
	public class MainApplication : MauiApplication<MyApp>
	{
		public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
		{
		}
	}
}