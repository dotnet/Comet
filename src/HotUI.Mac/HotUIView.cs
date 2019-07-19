using CoreGraphics;
using AppKit;
using HotUI.Mac.Extensions;
using HotUI.Mac.Handlers;
using System;

namespace HotUI.Mac
{
    public class HotUIView : NSColorView
    {
        private View _virtualView;
        private MacViewHandler _handler;
        private NSView _nativeView;
        
        public HotUIView()
        {
        }

        public HotUIView(CGRect rect) : base(rect)
        {

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
                    if (_handler is MacViewHandler viewHandler)
                        viewHandler.NativeViewChanged -= HandleNativeViewChanged;
                }

                _virtualView = value;

                if (_virtualView != null)
                {
                    _handler = _virtualView.GetOrCreateViewHandler();

                    _virtualView.ViewHandlerChanged += HandleViewHandlerChanged;
                    _virtualView.NeedsLayout += HandleNeedsLayout;
                    if (_handler is MacViewHandler viewHandler)
                        viewHandler.NativeViewChanged += HandleNativeViewChanged;

                    HandleNativeViewChanged(this, new ViewChangedEventArgs(_virtualView, null, (NSView)_handler.NativeView));
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

            if (e.OldViewHandler is MacViewHandler oldHandler)
            {
                oldHandler.NativeViewChanged -= HandleNativeViewChanged;
                _nativeView?.RemoveFromSuperview();
                _nativeView = null;
            }

            if (e.NewViewHandler is MacViewHandler newHandler)
            {
                newHandler.NativeViewChanged += HandleNativeViewChanged;
                _nativeView = newHandler.View ?? new NSView();
                AddSubview(_nativeView);
                SetNeedsLayout();
            }
        }

        private void HandleNativeViewChanged(object sender, ViewChangedEventArgs e)
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
                Layout();
            }
        }

        private void SetNeedsLayout()
        {
            NeedsLayout = true;
        }

        public override CGRect Frame
        {
            get => base.Frame;
            set
            {
                base.Frame = value;
                Layout();
                NeedsLayout = false;
            }
        }
        
        public override void ViewDidMoveToSuperview()
        {
            if (NeedsLayout)
            {
                Layout();
                NeedsLayout = false;
            }

            base.ViewDidMoveToSuperview();
        }

        public override void Layout()
        {
            if (Bounds.IsEmpty || _nativeView == null)
                return;

            if (_nativeView is NSScrollView sv)
            {
                _nativeView.Frame = Bounds;
            }
            else
            {
                var bounds = Bounds;
                
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
