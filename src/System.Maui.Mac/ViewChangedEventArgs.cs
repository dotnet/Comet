using System;
using AppKit;

namespace System.Maui.Mac
{
	public class ViewChangedEventArgs : EventArgs
	{
		public View VirtualView { get; }
		public NSView OldNativeView { get; }
		public NSView NewNativeView { get; }

		public ViewChangedEventArgs(
			View view,
			NSView oldNativeView,
			NSView newNativeView)
		{
			VirtualView = view;
			OldNativeView = oldNativeView;
			NewNativeView = newNativeView;
		}
	}
}
