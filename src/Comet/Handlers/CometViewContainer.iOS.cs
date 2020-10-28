using System;
using UIKit;
using Xamarin.Platform;

namespace Comet
{
	public class CometViewContainer : UIView, IReloadHandler
	{
		View parentView;
		View currentView;
		UIView childView;
		public void SetView(View view)
		{
			parentView = view;
			view.ReloadHandler ??= this;
			var v = view?.GetView();
			if (v == currentView)
				return;
			currentView = v;
			childView?.RemoveFromSuperview();
			childView = v.ToNative();
			if (childView != null)
				AddSubview(childView);
			view.InvalidateMeasurement();
		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			if (childView == null)
				return;
			childView.Frame = this.Bounds;
		}

		public void Reload() => parentView?.Parent?.InvalidateMeasurement();
	}
}
