using System;
using UIKit;
namespace HotUI.iOS {
	public static partial class iOSExtensions {


		static iOSExtensions()
		{
			HotUI.iOS.UI.Init ();
		}
		
		public static UIViewController ToViewController (this HotPage hotPage)
		{
			if (hotPage == null)
				return null;
			var handler = hotPage.ViewHandler;
			if (handler == null) {

				handler = Registrar.Pages.GetRenderer (hotPage.GetType ()) as IViewBuilderHandler;
				hotPage.ViewHandler = handler;
				hotPage.ReBuildView ();
			}
			var page = handler as IUIViewController;
			return page.ViewController;
		}

		public static UIView ToView (this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null) {

				handler = Registrar.Handlers.GetRenderer (view.GetType ()) as IViewHandler;
				view.ViewHandler = handler;
			}
			var page = handler as IUIView;
			return page.View;
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
