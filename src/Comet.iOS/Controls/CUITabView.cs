using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
namespace Comet.iOS
{
	public class CUITabView : UIView
	{
		UITabBarController tabViewController = new UITabBarController();
		public CUITabView()
		{
			Add(tabViewController.View);
		}
		public void Setup(List<View> views)
		{
			if (views == null)
			{
				tabViewController.ViewControllers = null;
				return;
			}
			var controllers = views.Select(x
				=> new Tuple<View, UIViewController>(x, x.ToViewController())).ToList();
			foreach (var pair in controllers)
			{
				var title = pair.Item1.GetEnvironment<string>(EnvironmentKeys.TabView.Title);
				var imagePath = pair.Item1.GetEnvironment<string>(EnvironmentKeys.TabView.Image);
				UIImage image = null;
				//TODO fix this do it so we can load from any source type;
				if (!string.IsNullOrWhiteSpace(imagePath))
					image = UIImage.FromBundle(imagePath);
				pair.Item2.TabBarItem = new UITabBarItem()
				{
					Title = title ?? "",
					Image = image,
				};
			};

			tabViewController.ViewControllers = controllers.Select(x => x.Item2).ToArray();
		}

		public override void MovedToSuperview()
		{
			base.MovedToSuperview();
			var vc = this.GetViewController();
			vc?.AddChildViewController(tabViewController);
		}
		public override void RemoveFromSuperview()
		{
			tabViewController.RemoveFromParentViewController();
			base.RemoveFromSuperview();
		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			tabViewController.View.Frame = this.Bounds;
		}
	}
}
