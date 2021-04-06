using System;
using CoreGraphics;
using UIKit;
using Microsoft.Maui.HotReload;
using Microsoft.Maui;

namespace Comet.iOS
{
	public class CometView : UIView, IReloadHandler
	{
		public CometView(IMauiContext mauiContext) {
			MauiContext = mauiContext;
		}
		public CometView(CGRect rect, IMauiContext mauiContext) : base(rect)
		{
			MauiContext = mauiContext;
		}
		IView _view;
		public IView CurrentView
		{
			get => _view;
			set => SetView(value);
		}
		public IMauiContext MauiContext { get; internal set; }

		UIView currentNativeView;
		void SetView(IView view, bool forceRefresh = false)
		{
			if (view == _view && !forceRefresh)
				return;
			_view = view;

			if (_view is IHotReloadableView ihr)
			{
				ihr.ReloadHandler = this;
				MauiHotReloadHelper.AddActiveView(ihr);
			}
			currentNativeView?.RemoveFromSuperview();
			currentNativeView = null;
			AddSubview(currentNativeView = _view.ToNative(MauiContext));
		}


		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			currentNativeView.Frame = Bounds;
		}



		public void Reload() => SetView(CurrentView, true);
	}
}
