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
			//reuse the handlers!
			if(view is View v && _view is View pv &&
				v.GetContentTypeHashCode() == pv.GetContentTypeHashCode()
				&& currentHandler != null)
			{
				_view = view;
				v.ViewHandler = currentHandler;
				if (_view is IHotReloadableView ihr1)
				{
					ihr1.ReloadHandler = this;
					MauiHotReloadHelper.AddActiveView(ihr1);
				}
				return;
			}

			_view = view;

			if (_view is IHotReloadableView ihr)
			{
				ihr.ReloadHandler = this;
				MauiHotReloadHelper.AddActiveView(ihr);
			}
			var newNativeView = _view?.ToNative(MauiContext);
			currentHandler = _view?.Handler;
			if (currentNativeView == newNativeView)
				return;
			currentNativeView?.RemoveFromSuperview();
			if (newNativeView != this && newNativeView != null)
				AddSubview(currentNativeView = newNativeView);
		}


		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			if (currentNativeView == null)
				return;
			_view?.Measure(Bounds.Width, Bounds.Height);// .LayoutSubviews(this.Bounds.ToRectangle());
			currentNativeView.Frame = Bounds;
		}



		public void Reload() => SetView(CurrentView, true);
	}
}
