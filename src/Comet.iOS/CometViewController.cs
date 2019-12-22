using System;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;

namespace Comet.iOS
{
    public class CometViewController : UIViewController
    {
        private CometView _containerView;
        private View _startingCurrentView;
        
        public View CurrentView
        {
            get => _containerView?.CurrentView ?? _startingCurrentView;
            set
            {
                if (_containerView != null)
                    _containerView.CurrentView = value;
                else
                    _startingCurrentView = value;

                Title = value?.GetEnvironment<string>(EnvironmentKeys.View.Title) ?? value?.BuiltView?.GetEnvironment<string>(EnvironmentKeys.View.Title) ?? "";

            }
        }

        public override void LoadView()
        {
            View = _containerView = new CometView(UIScreen.MainScreen.Bounds);
            _containerView.CurrentView = _startingCurrentView;
            _startingCurrentView = null;
        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            _containerView?.CurrentView?.ViewDidAppear();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ApplyStyle();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            _containerView?.CurrentView?.ViewDidDisappear();
        }

        public void ApplyStyle()
        {
            var barColor = _containerView?.CurrentView?.GetNavigationBackgroundColor();

            if (barColor != null && NavigationController != null)
            {
                this.NavigationController.NavigationBar.BarTintColor = barColor.ToUIColor();
            }
            
            var textColor = _containerView?.CurrentView?.GetNavigationTextColor();
            if (textColor != null && NavigationController != null)
            {
                var color = textColor.ToUIColor();
                this.NavigationController.NavigationBar.TintColor = color;
                this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes { ForegroundColor = color };
            }
        }
    }
}
