using System;
using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Comet.Samples;

namespace Maui.Controls.Sample.SingleProject
{
	[Application]
	public class MainApplication : MauiApplication<MyApp>
	{
		public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
		{
		}
	}
}