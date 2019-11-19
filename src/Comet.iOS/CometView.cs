using System;
using CoreGraphics;
using UIKit;

namespace Comet.iOS
{
    public class CometView : UIView
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

                if (_virtualView != null)
                {
                    _virtualView.ViewHandlerChanged -= HandleViewHandlerChanged;
                    _virtualView.NeedsLayout -= HandleNeedsLayout;
                    if (_handler is iOSViewHandler viewHandler)
                        viewHandler.NativeViewChanged -= HandleNativeViewChanged;
                }

                _virtualView = value;

                if (_virtualView != null)
                {
                    _handler = _virtualView.GetOrCreateViewHandler();
                    
                    _virtualView.ViewHandlerChanged += HandleViewHandlerChanged;
                    _virtualView.NeedsLayout += HandleNeedsLayout;
                    if (_handler is iOSViewHandler viewHandler)
                        viewHandler.NativeViewChanged += HandleNativeViewChanged;
                    
                    HandleNativeViewChanged(this, new ViewChangedEventArgs(_virtualView, null, (UIView)_handler.NativeView));
                }
            }
        }
        
        private void HandleNeedsLayout(object sender, EventArgs e)
        {
            SetNeedsLayout();
        }

        private void HandleViewHandlerChanged(object sender, ViewHandlerChangedEventArgs e)
        {
            Console.WriteLine($"[{GetType().Name}] HandleViewHandlerChanged: [{sender.GetType()}] From:[{e.OldViewHandler?.GetType()}] To:[{e.NewViewHandler?.GetType()}]");

            if (e.OldViewHandler is iOSViewHandler oldHandler)
            {
                oldHandler.NativeViewChanged -= HandleNativeViewChanged;
                _nativeView?.RemoveFromSuperview();
                _nativeView = null;
            }

            if (e.NewViewHandler is iOSViewHandler newHandler)
            {
                newHandler.NativeViewChanged += HandleNativeViewChanged;
                _nativeView = newHandler.View ?? new UIView();
                AddSubview(_nativeView);
                SetNeedsLayout();
            }
        }

        void HandleNativeViewChanged(object sender, ViewChangedEventArgs args)
        {
            if (_virtualView == null)
                return;

            var newNativeView = _handler?.View;
            if (newNativeView == _nativeView)
                return;

            var _previousFrame = _nativeView?.Frame;
            _nativeView?.RemoveFromSuperview();
            _nativeView = newNativeView;

            if (newNativeView != null)
            {
                if (_previousFrame != null)
                    newNativeView.Frame =  (CGRect)_previousFrame;
                AddSubview(newNativeView);
                SetNeedsLayout();
            }
        }
        
        public override void LayoutSubviews()
        {
            if (Bounds.IsEmpty || _nativeView == null)
                return;
            var iOSHandler = _virtualView?.BuiltView?.ViewHandler as iOSViewHandler;

            bool ignoreSafeArea = iOSHandler?.IgnoreSafeArea ?? false ;

            var bounds = Bounds;

            if(ignoreSafeArea)
                _nativeView.Frame = Bounds;
            else
            {
                //TODO: opt out of safe are
                var safe = SafeAreaInsets;
                bounds.X += safe.Left;
                bounds.Y += safe.Top;
                bounds.Height -= safe.Top + safe.Bottom;
                bounds.Width -= safe.Left + safe.Right;
                _virtualView.SetFrameFromNativeView(bounds.ToRectangleF());
            }
           
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
                CurrentView?.Dispose();
            base.Dispose(disposing);
        }
    }
}
