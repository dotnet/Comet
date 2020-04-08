using System;
using AView = Android.Views.View;

namespace System.Maui.Android
{
	public class ViewChangedEventArgs : EventArgs
	{
		public View VirtualView { get; }
		public AView OldNativeView { get; }
		public AView NewNativeView { get; }

		public ViewChangedEventArgs(
			View view,
			AView oldNativeView,
			AView newNativeView)
		{
			VirtualView = view;
			OldNativeView = oldNativeView;
			NewNativeView = newNativeView;
		}
	}
}
