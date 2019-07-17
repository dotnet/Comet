using System;
using UIKit;

namespace HotUI.iOS
{
    public class HUINavigationController : UINavigationController
    {
        public HUINavigationController()
        {
        }
        public override UIViewController[] PopToRootViewController(bool animated)
        {
            return base.PopToRootViewController(animated);
        }
        public override UIViewController PopViewController(bool animated)
        {
            var vc = base.PopViewController(animated);
            var hotVC = vc as HotUIViewController;
            if (hotVC?.CurrentView != null) {
                hotVC?.CurrentView?.Dispose();
                hotVC.CurrentView = null;
            }
            return vc;


        }
    }
}
