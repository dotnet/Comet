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
		AView currentPlatformView;
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

			_view = view;


			if (_view is IHotReloadableView ihr)
			{
				ihr.ReloadHandler = this;
				MauiHotReloadHelper.AddActiveView(ihr);
			}
			var newPlatformView = _view?.ToPlatform(MauiContext);

			if (view is IReplaceableView ir)
				currentHandler = ir.ReplacedView.Handler;
			else
				currentHandler = _view?.Handler;
			if (currentPlatformView == newPlatformView)
				return;
			if (currentPlatformView != null)
				RemoveView(currentPlatformView);
			if (_view == null)
				return;

			currentPlatformView = currentHandler.PlatformView as AView ?? new AView(MauiContext.Context);
			if (currentPlatformView.Parent == this)
				return;
			if (currentPlatformView.Parent != null)
				(currentPlatformView.Parent as AViewGroup).RemoveView(currentPlatformView);
			AddView(currentPlatformView, new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));

		}

		private void HandleNeedsLayout(object sender, EventArgs e)
		{
			RequestLayout();
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			var deviceIndependentWidth = widthMeasureSpec.ToDouble(Context);
			var deviceIndependentHeight = heightMeasureSpec.ToDouble(Context);
			var size = CurrentView.Measure(deviceIndependentWidth, deviceIndependentHeight);
			var nativeWidth = Context.ToPixels(size.Width);
			var nativeHeight = Context.ToPixels(size.Height);
			SetMeasuredDimension((int)nativeWidth, (int)nativeHeight);
		}

		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			if (currentPlatformView == null || inLayout) return;

			var displayScale = CometApp.CurrentWindow.DisplayScale;
			var width = (right - left) / displayScale;
			var height = (bottom - top) / displayScale;
			if (width > 0 && height > 0)
			{
				inLayout = true;
				var rect = new Rect(0, 0, width, height);
				CurrentView.Arrange(rect); 
				inLayout = false;
			}
		}
		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);
			if (w > 0 && h > 0)
			{
				inLayout = true;
				var rect = new Rect(0, 0, w, h);
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