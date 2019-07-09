using CoreGraphics;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac
{
    public class HotUIView : NSColorView
    {
        private View _virtualView;
        private MacViewHandler _handler;
        private NSView _nativeView;
        
        public HotUIView()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            AutoresizesSubviews = true;
            AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
        }

        public HotUIView(CGRect rect) : base(rect)
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            AutoresizesSubviews = true;
            AutoresizingMask = NSViewResizingMask.HeightSizable | NSViewResizingMask.WidthSizable;
        }

        public View CurrentView
        {
            get => _virtualView;
            set
            {
                if (value == _virtualView)
                    return;

                _virtualView = value;
                _handler = _virtualView.ToINSView();
                if (_handler is ViewHandler viewHandler)
                    viewHandler.ViewChanged = HandleViewChanged;

                HandleViewChanged();
            }
        }


        void HandleViewChanged()
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

                if (_nativeView is NSTableView || _nativeView is ListViewHandler)
                    _nativeView.Frame = bounds;
                else
                {
                    CGSize sizeThatFits;
                    if (_nativeView is AbstractLayoutHandler layout)
                        sizeThatFits = layout.SizeThatFits(bounds.Size);
                    else
                        sizeThatFits = bounds.Size;
                    var x = ((bounds.Width - sizeThatFits.Width) / 2) + padding.Left;
                    var y = ((bounds.Height - sizeThatFits.Height) / 2) + padding.Top;
                    _nativeView.Frame = new CGRect(x, y, sizeThatFits.Width, sizeThatFits.Height);
                }
            }
        }
    }
}
