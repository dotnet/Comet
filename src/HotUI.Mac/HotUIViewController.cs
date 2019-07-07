using System;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac {
	public class HotUIViewController : NSViewController {

		public HotUIViewController()
		{
			View = new NSView ();
		}
		INSView currentView;
		public INSView CurrentView {
			get => currentView;
			set {
				if (value == currentView)
					return;
				currentView = value;
				if (currentView is ViewHandler vh) {
					vh.ViewChanged = SetView;
				}

				SetView ();
			}
		}

		NSView currentlyShownView;
		void SetView ()
		{
			var view = CurrentView?.View;
			if (view == currentlyShownView)
				return;
			currentlyShownView?.RemoveFromSuperview ();
			currentlyShownView = view;
			if (view == null)
				return;
			View.AddSubview(view);
		}
		public override void ViewDidLayout ()
		{
			base.ViewDidLayout ();
			if (currentlyShownView != null)
				currentlyShownView.Frame = View.Bounds;
		}
	}
}
