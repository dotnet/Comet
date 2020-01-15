using System;
using UIKit;

namespace Comet.iOS
{
	public class CUINavigationController : UINavigationController
	{
		public static UIColor DefaultBarTintColor { get; private set; }
		public static UIColor DefaultTintColor { get; private set; }
		public static UIStringAttributes DefaultTitleTextAttributes { get; private set; }
		public CUINavigationController()
		{
			if (DefaultBarTintColor == null)
			{
				DefaultBarTintColor = NavigationBar.BarTintColor;
				DefaultTintColor = NavigationBar.TintColor;
				DefaultTitleTextAttributes = NavigationBar.TitleTextAttributes;
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
			cometVC?.WasPopped();
		
			return vc;
		}
	}
}
