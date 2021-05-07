using System;
using AContext = Android.Content.Context;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;
using Microsoft.Maui;
using Microsoft.Maui.HotReload;
using Microsoft.Maui.Graphics;

namespace Comet.Android
{
	public class CometView : AViewGroup, IReloadHandler
	{
		IView _view;
		IViewHandler currentHandler;
		AView currentNativeView;
		private bool inLayout;

		IMauiContext MauiContext;
		public CometView(IMauiContext mc) : base(mc.Context)
		{
			MauiContext = mc;
		}

		public IView CurrentView
		{
			get => _view;
			set => SetView(value);
		}

		void SetView(IView view, bool forceRefresh = false)
		{
			if (view == _view && !forceRefresh)
				return;
			//If the views are the same type- reuse the handlers!
			if (view is View v && _view is View pv && v.AreSameType(pv, true) && currentHandler != null)
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
			if (currentNativeView != null)
				RemoveView(currentNativeView);

			currentNativeView = currentHandler.NativeView as AView ?? new AView(MauiContext.Context);
			AddView(currentNativeView, new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));

		}

		private void HandleNeedsLayout(object sender, EventArgs e)
		{
			RequestLayout();
		}


		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			if (currentNativeView == null || inLayout) return;

			var displayScale = CometApp.CurrentWindow.DisplayScale;
			var width = (right - left) / displayScale;
			var height = (bottom - top) / displayScale;
			if (width > 0 && height > 0)
			{
				inLayout = true;
				var rect = new Rectangle(0, 0, width, height);
				CurrentView.Arrange(rect); 
				inLayout = false;
			}
		}

		protected override void Dispose(bool disposing)
		{
			//if (disposing)
			//	CurrentView?.Dispose();
			base.Dispose(disposing);
		}
		public void Reload() => SetView(CurrentView, true);
	}
}