using System;
using AView = global::Android.Views.View;
using System.Maui.Android.Controls;
using LP = Android.Views.ViewGroup.LayoutParams;

namespace System.Maui.Android.Handlers
{
	public class MauiContentView : CustomFrameLayout
	{
		public MauiContentView() : base(AndroidContext.CurrentContext)
		{
		}
	}
}
