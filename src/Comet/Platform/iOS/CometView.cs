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
		IViewHandler currentHandler;
		void SetView(IView view, bool forceRefresh = false)
		{
			if (view == _view && !forceRefresh)
				return;
			//If the views are the same type- reuse the handlers!
			if(view is View v && _view is View pv && v.AreSameType(pv,true) && currentHandler != null)
			{
				_view = view;
				v.ViewHandler = currentHandler;
				currentHandler.SetVirtualView(view);
				return;
			}

			_view = view;

			if (_view is IHotReloadableView ihr)
			{
				ihr.ReloadHandler = this;
				MauiHotReloadHelper.AddActiveView(ihr);
			}
			var newNativeView = _view.ToNative(MauiContext);
			currentHandler = _view.Handler;
			if (currentNativeView == newNativeView)
				return;
			currentNativeView?.RemoveFromSuperview();
			AddSubview(currentNativeView = newNativeView);
		}


		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			currentNativeView.Frame = Bounds;
		}



		public void Reload() => SetView(CurrentView, true);
	}
}
