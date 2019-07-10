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
            TranslatesAutoresizingMaskIntoConstraints = false;
        }

        public HotUIView(CGRect rect) : base(rect)
        {
            BackgroundColor = UIColor.White;
            TranslatesAutoresizingMaskIntoConstraints = false;
        }

        public View CurrentView
        {
            get => _virtualView;
            set
            {
                if (value == _virtualView)
                    return;

                _virtualView = value;
                _handler = _virtualView.ToIUIView();
                if (_handler is ViewHandler viewHandler)
                    viewHandler.NativeViewChanged += HandleViewChanged;

                HandleViewChanged(this, new ViewChangedEventArgs(_virtualView,null,null));
            }
        }


        void HandleViewChanged(object sender, ViewChangedEventArgs args)
        {
            if (_virtualView == null)
                return;

            var newNativeView = _handler?.View;
            if (newNativeView == _nativeView)
                return;

            _nativeView?.RemoveFromSuperview();
            _nativeView = newNativeView;

            if (newNativeView != null)
                AddSubview(newNativeView);

            SetNeedsLayout();
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

                if (_nativeView is UITableView)
                    _nativeView.Frame = bounds;
                else
                {
                    var sizeThatFits = _nativeView.SizeThatFits(bounds.Size);
                    var x = ((bounds.Width - sizeThatFits.Width) / 2) + padding.Left;
                    var y = ((bounds.Height - sizeThatFits.Height) / 2) + padding.Top;
                    _nativeView.Frame = new CGRect(x, y, sizeThatFits.Width, sizeThatFits.Height);
                }
            }
        }
    }
}
