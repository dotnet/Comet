using System;
using CoreGraphics;
using UIKit;

namespace Comet.iOS
{
	public class CometView : UIView, IReloadHandler
	{
		private View _virtualView;
		private iOSViewHandler _handler;
		private UIView _nativeView;

		public CometView()
		{
			BackgroundColor = UIColor.SystemBackgroundColor;
		}

		public CometView(CGRect rect) : base(rect)
		{
			BackgroundColor = UIColor.SystemBackgroundColor;
		}

		public View CurrentView
		{
			get => _virtualView;
			set
			{
				if (value == _virtualView)
					return;

				UIView previousView = null;
				if (_virtualView != null)
				{
					previousView = (UIView)_virtualView.ViewHandler?.NativeView;
					_virtualView.ViewHandlerChanged -= HandleViewHandlerChanged;
					_virtualView.NeedsLayout -= HandleNeedsLayout;
					if (_handler is iOSViewHandler viewHandler)
						viewHandler.NativeViewChanged -= HandleNativeViewChanged;
				}

				_virtualView = value;
				_handler = _virtualView.GetOrCreateViewHandler();

				if (_virtualView != null)
				{
					_virtualView.ReloadHandler ??= this;
					_virtualView.ViewHandlerChanged += HandleViewHandlerChanged;
					_virtualView.NeedsLayout += HandleNeedsLayout;
					if (_handler is iOSViewHandler viewHandler)
						viewHandler.NativeViewChanged += HandleNativeViewChanged;
				}

				HandleNativeViewChanged(this, new ViewChangedEventArgs(_virtualView, previousView, (UIView)_handler?.NativeView));
			}
		}

		private void HandleNeedsLayout(object sender, EventArgs e)
		{
			SetNeedsLayout();
		}

		private void HandleViewHandlerChanged(object sender, ViewHandlerChangedEventArgs e)
		{
			Logger.Debug($"[{GetType().Name}] HandleViewHandlerChanged: [{sender.GetType()}] From:[{e.OldViewHandler?.GetType()}] To:[{e.NewViewHandler?.GetType()}]");
			var oldHandler = e.OldViewHandler as iOSViewHandler ?? _handler;
			if (oldHandler != null)
			{
				oldHandler.NativeViewChanged -= HandleNativeViewChanged;
				_nativeView?.RemoveFromSuperview();
				_nativeView = null;
				_handler = null;
			}

			if (e.NewViewHandler is iOSViewHandler newHandler)
			{
				_handler = newHandler;
				newHandler.NativeViewChanged += HandleNativeViewChanged;
				_nativeView = newHandler.View ?? new UIView();
				AddSubview(_nativeView);
				SetNeedsLayout();
			}
		}

		void HandleNativeViewChanged(object sender, ViewChangedEventArgs args)
		{
			CGRect? previousFrame = null;
			if (args.OldNativeView != null)
			{
				previousFrame = args.OldNativeView.Frame;
				args.OldNativeView.RemoveFromSuperview();
				_nativeView = null;
			}

			if (args.NewNativeView != null)
			{
				_nativeView = args.NewNativeView;
				if (_nativeView.Superview != null)
					_nativeView.RemoveFromSuperview();

				if (previousFrame != null)
					_nativeView.Frame = (CGRect)previousFrame;
				AddSubview(_nativeView);
				SetNeedsLayout();
			}
		}

		public override void LayoutSubviews()
		{
			if (Bounds.IsEmpty || _nativeView == null || _virtualView == null)
				return;
			var iOSHandler = (_virtualView?.ViewHandler ?? _virtualView?.BuiltView?.ViewHandler) as iOSViewHandler;

			bool ignoreSafeArea = iOSHandler?.IgnoreSafeArea ?? false;

			var bounds = Bounds;

			if (ignoreSafeArea)
			{
				_virtualView.SetFrameFromNativeView(Bounds.ToRectangleF());
			}
			else
			{
				//TODO: opt out of safe are
				var safe = SafeAreaInsets;
				bounds.X += safe.Left;
				bounds.Y += safe.Top;
				bounds.Height -= safe.Top + safe.Bottom;
				bounds.Width -= safe.Left + safe.Right;
				_virtualView?.SetFrameFromNativeView(bounds.ToRectangleF());
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				CurrentView?.Dispose();
			base.Dispose(disposing);
		}

		public void Reload()
		{
			//TODO: Fix this!

			UIView previousView = _handler?.View;
			_handler = _virtualView.GetOrCreateViewHandler();
			HandleNativeViewChanged(this, new ViewChangedEventArgs(_virtualView, previousView, (UIView)_handler?.NativeView));

		}
	}
}
