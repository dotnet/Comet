using Comet.iOS;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using UIKit;

namespace Comet.Handlers
{
	public partial class NavigationViewHandler : ViewHandler<NavigationView, UIView>, INativeViewHandler
	{
		public NavigationViewHandler() : base(ViewHandler.ViewMapper)
		{

		}
		UIViewController viewController;
		UIViewController INativeViewHandler.ViewController => viewController;
		protected override UIView CreateNativeView()
		{
			var vc = VirtualView.Content.ToUIViewController(MauiContext);
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
				var newVc = toView.ToUIViewController(MauiContext);
				navigationController.PushViewController(newVc, true);
			});
			nav.SetPerformPop(() => navigationController.PopViewController(true));
			navigationController.PushViewController(vc, true);

			return navigationController.View;
		}
	}
}
