using System;
using UIKit;

namespace System.Maui.iOS
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
			var mauiVC = vc as MauiViewController;
			mauiVC?.WasPopped();
		
			return vc;
		}
	}
}
