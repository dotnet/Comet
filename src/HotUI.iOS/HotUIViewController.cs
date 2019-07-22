using System;
using System.Threading.Tasks;
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

                Title = value?.GetEnvironment<string>(EnvironmentKeys.View.Title) ?? "";
            }
        }

        public override void LoadView()
        {
            View = _containerView = new HotUIView(UIScreen.MainScreen.Bounds);
            _containerView.CurrentView = _startingCurrentView;
            _startingCurrentView = null;
        }
    }
}