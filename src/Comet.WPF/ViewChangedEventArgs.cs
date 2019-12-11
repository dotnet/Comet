using System;
using System.Windows;

namespace Comet.WPF
{
	public class ViewChangedEventArgs : EventArgs
	{
		public View VirtualView { get; }
		public UIElement OldNativeView { get; }
		public UIElement NewNativeView { get; }

		public ViewChangedEventArgs(
			View view,
			UIElement oldNativeView,
			UIElement newNativeView)
		{
			VirtualView = view;
			OldNativeView = oldNativeView;
			NewNativeView = newNativeView;
		}
	}
}
