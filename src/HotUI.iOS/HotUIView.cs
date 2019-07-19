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
            AutosizesSubviews = false;
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

            if (_nativeView is UIScrollView sv)
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

                var padding = _virtualView.GetPadding();
                if (!padding.IsEmpty)
                {
                    bounds.X += padding.Left;
                    bounds.Y += padding.Top;
                    bounds.Width -= padding.HorizontalThickness;
                    bounds.Height -= padding.VerticalThickness;
                }

                var sizeThatFits = _virtualView.Measure(bounds.Size.ToSizeF());
                _virtualView.MeasuredSize = sizeThatFits;
                _virtualView.MeasurementValid = true;
                
                var x = ((bounds.Width - sizeThatFits.Width) / 2) + padding.Left;
                var y = ((bounds.Height - sizeThatFits.Height) / 2) + padding.Top;
                _virtualView.Frame = new RectangleF((float)x, (float)y, sizeThatFits.Width, sizeThatFits.Height);
            }
        }
    }
}
