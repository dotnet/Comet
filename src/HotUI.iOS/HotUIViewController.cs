using CoreGraphics;
using UIKit;

namespace HotUI.iOS
{
    public class HotUIViewController : UIViewController
    {
        private HotUIView _containerView;
        private View _startingCurrentView;
        
        public HotUIViewController()
        {
        }

        public View CurrentView
        {
            get => _containerView?.CurrentView ?? _startingCurrentView;
            set
            {
                if (_containerView != null)
                    _containerView.CurrentView = value;
                else
                    _startingCurrentView = value;
            }
        }

        public override void LoadView()
        {
            base.LoadView();
            
            var containerView = _containerView = new HotUIView(View.Bounds);
            _containerView.CurrentView = _startingCurrentView;
            _startingCurrentView = null;
            containerView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            View.AutosizesSubviews = true;
            
            View.AddSubview(containerView);
        }
    }
}