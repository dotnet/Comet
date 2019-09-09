using System;
using UIKit;

namespace Comet.iOS
{
    public class CUINavigationController : UINavigationController
    {
        public CUINavigationController()
        {
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
