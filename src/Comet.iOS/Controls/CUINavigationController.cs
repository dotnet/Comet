using System;
using UIKit;

namespace Comet.iOS
{
    public class CUINavigationController : UINavigationController
    {
        static UIColor DefaultBarTintColor;
        public CUINavigationController()
        {
            if (DefaultBarTintColor == null)
            {
                DefaultBarTintColor = NavigationBar.BarTintColor;
            }
        }
        public override UIViewController[] PopToRootViewController(bool animated)
        {
            return base.PopToRootViewController(animated);
        }
        public override UIViewController PopViewController(bool animated)
        {
            var vc = base.PopViewController(animated);
            var cometVC = vc as CometViewController;
            if (cometVC?.CurrentView != null) {
                cometVC?.CurrentView?.Dispose();
                cometVC.CurrentView = null;
            }
            return vc;
        }
    }
}
