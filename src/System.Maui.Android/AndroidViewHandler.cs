using System;
using System.Maui.Android.Controls;
using AView = Android.Views.View;

namespace System.Maui.Android
{
	public interface AndroidViewHandler : IViewHandler
	{
		event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		AView View { get; }

		MauiTouchGestureListener GestureListener { get; set; }
	}
}
