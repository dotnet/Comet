using Comet.iOS;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using UIKit;

namespace Comet.Handlers
{
	public partial class NavigationViewHandler : ViewHandler<NavigationView, UIView>, IPlatformViewHandler
	{
		UIViewController viewController;
		UIViewController IPlatformViewHandler.ViewController => viewController;
		protected override UIView CreatePlatformView()
		{
			var vc = new Comet.iOS.CometViewController { MauiContext = MauiContext, CurrentView = VirtualView.Content };
			var nav = VirtualView;
			if (nav.Navigation != null)
			{
				viewController = vc;
				return viewController.View;
			}
			var navigationController = new CUINavigationController();
			viewController = navigationController;
			nav.SetPerformNavigate((toView) => {
				if (toView is NavigationView newNav)
				{
					newNav.SetPerformNavigate(nav);
					newNav.SetPerformPop(nav);
				}

				toView.Navigation = nav;
				var newVc = new Comet.iOS.CometViewController { MauiContext = MauiContext, CurrentView = toView };
				navigationController.PushViewController(newVc, true);
			});
			nav.SetPerformPop(() => navigationController.PopViewController(true));
			navigationController.PushViewController(vc, true);

			return navigationController.View;
		}
	}
}
