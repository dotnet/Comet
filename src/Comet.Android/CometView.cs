using System;
using System.Drawing;
using AContext = Android.Content.Context;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;

namespace Comet.Android
{
	public class CometView : AViewGroup
	{
		View virtualView;
		AndroidViewHandler handler;
		AView nativeView;
		private bool inLayout;

		public CometView(AContext context) : base(context)
		{
		}

		public View CurrentView
		{
			get => virtualView;
			set
			{
				if (value == virtualView)
					return;

				AView previousView = null;
				if (virtualView != null)
				{
					previousView = (AView)virtualView.ViewHandler?.NativeView;
					virtualView.ViewHandlerChanged -= HandleViewHandlerChanged;
					virtualView.NeedsLayout -= HandleNeedsLayout;
					if (handler is AndroidViewHandler viewHandler)
						viewHandler.NativeViewChanged -= HandleNativeViewChanged;
				}

				virtualView = value;
				handler = virtualView?.GetOrCreateViewHandler();

				if (virtualView != null)
				{
					virtualView.ViewHandlerChanged += HandleViewHandlerChanged;
					virtualView.NeedsLayout += HandleNeedsLayout;
					if (handler is AndroidViewHandler viewHandler)
						viewHandler.NativeViewChanged += HandleNativeViewChanged;
				}

				HandleNativeViewChanged(this, new ViewChangedEventArgs(virtualView, previousView, (AView)handler?.NativeView));
			}
		}

		private void HandleNeedsLayout(object sender, EventArgs e)
		{
			RequestLayout();
		}

		private void HandleViewHandlerChanged(object sender, ViewHandlerChangedEventArgs e)
		{
			Logger.Debug($"[{GetType().Name}] HandleViewHandlerChanged: [{sender.GetType()}] From:[{e.OldViewHandler?.GetType()}] To:[{e.NewViewHandler?.GetType()}]");

			if (e.OldViewHandler is AndroidViewHandler oldHandler)
			{
				oldHandler.NativeViewChanged -= HandleNativeViewChanged;
				RemoveView(oldHandler.View);
				nativeView = null;
				handler = null;
			}

			if (e.NewViewHandler is AndroidViewHandler newHandler)
			{
				handler = newHandler;
				handler.NativeViewChanged += HandleNativeViewChanged;
				nativeView = handler.View ?? new AView(AndroidContext.CurrentContext);
				AddView(nativeView, new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));
			}
		}

		void HandleNativeViewChanged(object sender, ViewChangedEventArgs args)
		{
			if (args.OldNativeView != null)
			{
				RemoveView(args.OldNativeView);
				nativeView = null;
			}

			if (args.NewNativeView != null)
			{
				nativeView = args.NewNativeView;
				if (nativeView.Parent is AViewGroup viewGroup)
					viewGroup.RemoveView(nativeView);
				AddView(nativeView, new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));
			}
		}

		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			if (nativeView == null || inLayout) return;

			var displayScale = AndroidContext.DisplayScale;
			var width = (right - left) / displayScale;
			var height = (bottom - top) / displayScale;
			if (width > 0 && height > 0)
			{
				inLayout = true;
				var rect = new RectangleF(0, 0, width, height);
				virtualView.SetFrameFromNativeView(rect);
				inLayout = false;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				CurrentView?.Dispose();
			base.Dispose(disposing);
		}
	}
}