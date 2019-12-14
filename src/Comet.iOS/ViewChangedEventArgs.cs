using System;
using UIKit;

namespace Comet.iOS
{
	public class ViewChangedEventArgs : EventArgs
	{
		public View VirtualView { get; }
		public UIView OldNativeView { get; }
		public UIView NewNativeView { get; }

		public ViewChangedEventArgs(
			View view,
			UIView oldNativeView,
			UIView newNativeView)
		{
			VirtualView = view;
			OldNativeView = oldNativeView;
			NewNativeView = newNativeView;
		}
	}
}
