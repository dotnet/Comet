using System;
using System.Linq;
using UIKit;
namespace HotUI.iOS {
	public static partial class iOSExtensions {


		static iOSExtensions()
		{
			HotUI.iOS.UI.Init ();
		}
		public static UIViewController ToViewController (this View view, bool allowNav = true)
		{
			var handler = view.ToIUIView ();
			
			var vc = new HotUIViewController {
				CurrentView = handler,
			};
			if (view.BuiltView is NavigationView nav && allowNav) {
				var navController = new UINavigationController ();
				nav.Navigate = (toView) => {
					//Since iOS doesn't allow nested navigations, pass the navigate along
					if(toView is NavigationView newNav) {
						newNav.Navigate = nav.Navigate;
					}
					navController.PushViewController (toView.ToViewController (false), true);
				};
				navController.PushViewController (vc, false);
				return navController;
			}
			return vc;
		}
		public static IUIView ToIUIView(this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null) {

				handler = Registrar.Handlers.GetRenderer (view.GetType ()) as IViewHandler;
				view.ViewHandler = handler;
			}
			var iUIView = handler as IUIView;
			return iUIView;
		}
		public static UIView ToView (this View view)
		{
			var handler = view.ToIUIView ();
			return handler?.View;
		}


		//public static LayoutOptions ToContentMode (this UIViewContentMode content, bool isVertical )
		//{
		//	bool isExpand = content.HasFlag (UIViewContentMode.ScaleToFill);
		//	switch (content) {
		//	case UIViewContentMode.Bottom:
		//		return  (isVertical ? LayoutOptions.End : LayoutOptions.Center).WithExpand(isExpand);
		//	case UIViewContentMode.BottomLeft:
		//		return (isVertical ? LayoutOptions.End : LayoutOptions.Start).WithExpand(isExpand);
		//	case UIViewContentMode.BottomRight:
		//		return LayoutOptions.End.WithExpand(isExpand);
		//	case UIViewContentMode.Center:
		//		return LayoutOptions.Center;
		//	case UIViewContentMode.Left:
		//		return isVertical ? LayoutOptions.Center : LayoutOptions.Start;
		//	case UIViewContentMode.Right:
		//	case UIViewContentMode.ScaleAspectFill:
		//	case UIViewContentMode.Top:
		//	case UIViewContentMode.TopLeft:
		//	case UIViewContentMode.TopRight:

		//	}
		//}

		//static LayoutOptions WithExpand (this LayoutOptions layoutOptions, bool isExpand)
		//{
		//	if (!isExpand)
		//		return layoutOptions;
		//	switch(layoutOptions) {
		//	case LayoutOptions.End:
		//		return LayoutOptions.EndAndExpand;
		//	case LayoutOptions.Center:
		//		return LayoutOptions.CenterAndExpand:
		//		case 
		//	}

		//}
		//public static UIViewContentMode ToContentMode(this LayoutOptions )
		//{
		//	switch(layoutOptions) {
		//	case LayoutOptions.Center:
		//	case LayoutOptions.CenterAndExpand:
		//	case LayoutOptions.End:
		//	case LayoutOptions.EndAndExpand:
		//	case LayoutOptions.Fill:
		//	case LayoutOptions.FillAndExpand:
		//	case LayoutOptions.Start:
		//	case LayoutOptions.StartAndExpand:
		//		return 
		//	}
		//}

	}
}
