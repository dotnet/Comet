using System;
using CoreGraphics;
using UIKit;

namespace HotUI.iOS
{
    public class HotUIView : UIView
    {
        private View _virtualView;
        private iOSViewHandler _handler;
        private UIView _nativeView;

        public HotUIView()
        {
            BackgroundColor = UIColor.White;
        }

        public HotUIView(CGRect rect) : base(rect)
        {
            BackgroundColor = UIColor.White;
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

            _nativeView?.RemoveFromSuperview();
            _nativeView = newNativeView;

            if (newNativeView != null)
            {
                AddSubview(newNativeView);
                SetNeedsLayout();
            }
        }
        
        public override void LayoutSubviews()
        {
            if (Bounds.IsEmpty || _nativeView == null)
                return;
            var iOSHandler = _virtualView?.BuiltView?.ViewHandler as iOSViewHandler;

            bool autoAdjust = iOSHandler?.AutoSafeArea ?? true ;

            if (!autoAdjust || _nativeView is UIScrollView)
            {
                _nativeView.Frame = Bounds;
            }
            else
            {
                //TODO: opt out of safe are
                var bounds = Bounds;
                var safe = SafeAreaInsets;
                bounds.X += safe.Left;
                bounds.Y += safe.Top;
                bounds.Height -= safe.Top + safe.Bottom;
                bounds.Width -= safe.Left + safe.Right;

                _virtualView.SetFrameFromNativeView(bounds.ToRectangleF());
            }
        }
    }
}
