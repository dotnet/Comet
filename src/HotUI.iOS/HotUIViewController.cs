using CoreGraphics;
using UIKit;

namespace HotUI.iOS
{
    public class HotUIViewController : UIViewController
    {
        public HotUIViewController()
        {
        }

        private View _view;
        private IUIView _handler;

        public View CurrentView
        {
            get => _view;
            set
            {
                
                if (value == _view)
                    return;

                _view = value;
                _handler = _view.ToIUIView();
                if (_handler is ViewHandler vh)
                    vh.ViewChanged = SetView;

                SetView();
            }
        }

        UIView currentlyShownView;

        void SetView()
        {
            if (ViewIfLoaded == null || CurrentView == null)
                return;
            
            var view = _handler?.View;
            if (view == currentlyShownView)
                return;
            currentlyShownView?.RemoveFromSuperview();
            currentlyShownView = view;
            if (view == null)
                return;
            View.AddSubview(view);
        }

        public override void LoadView()
        {
            base.LoadView();
            View.BackgroundColor = UIColor.White;
            SetView();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            if (currentlyShownView == null)
                return;
            if (currentlyShownView is UIScrollView sv)
            {
                currentlyShownView.Frame = View.Bounds;
            }
            else
            {
                //TODO: opt out of safe are
                var bounds = View.Bounds;
                var safe = View.SafeAreaInsets;
                bounds.X += safe.Left;
                bounds.Y += safe.Top;
                bounds.Height -= safe.Top + safe.Bottom;
                bounds.Width -= safe.Left + safe.Right;

                var padding = _view.GetPadding();
                if (!padding.IsEmpty)
                {
                    bounds.X += padding.Left;
                    bounds.Y += padding.Top;
                    bounds.Width -= padding.HorizontalThickness;
                    bounds.Height -= padding.VerticalThickness;
                }
                
                if (currentlyShownView is UITableView lv)
                    currentlyShownView.Frame = bounds;
                else
                    currentlyShownView.Center = new CGPoint(bounds.GetMidX(), bounds.GetMidY());
            }
        }
    }
}