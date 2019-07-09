using System;
using System.Linq;
using CoreGraphics;
using UIKit;
namespace HotUI.iOS {
	public static partial class iOSExtensions {


		static iOSExtensions()
		{
			HotUI.iOS.UI.Init ();
		}
		public static UIViewController ToViewController (this View view, bool allowNav = true)
		{
			var vc = new HotUIViewController 
			{
				CurrentView = view,
			};
			if (view.BuiltView is NavigationView nav && allowNav) {
				var navController = new UINavigationController ();
				nav.PerformNavigate = (toView) => {
					//Since iOS doesn't allow nested navigations, pass the navigate along
					if(toView is NavigationView newNav) {
						newNav.PerformNavigate = nav.PerformNavigate;
					}
					navController.PushViewController (toView.ToViewController (false), true);
				};
				navController.PushViewController (vc, false);
				return navController;
			}
			return vc;
		}
		public static iOSViewHandler ToIUIView(this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null) {

				handler = Registrar.Handlers.GetRenderer (view.GetType ()) as IViewHandler;
				view.ViewHandler = handler;
			}
			var iUIView = handler as iOSViewHandler;
			return iUIView;
		}
		public static UIView ToView (this View view)
		{
			var handler = view.ToIUIView ();
			return handler?.View;
		}

        public static Color ToColor(this UIColor color)
        {
            if (color == null)
                return null;

            color.GetRGBA(out var red, out var green, out var blue, out var alpha);
            return new Color((float)red, (float)green, (float)blue, (float)alpha);
        }

        public static UIColor ToUIColor(this Color color)
        {
            if (color == null)
                return null;

            return new UIColor(color.R, color.G, color.B, color.A);
        }

        public static CGColor ToCGColor(this Color color)
        {
	        if (color == null)
		        return null;

	        return new CGColor(color.R, color.G, color.B, color.A);
        }
        
        public static Font ToFont(this UIFont font)
        {
	        if (font == null)
		        return Font.System(12);

	        // todo: implement support for attributes other than name and size.
	        return font.Name == Device.FontService.SystemFontName 
		        ? Font.System((float)font.PointSize) 
		        : Font.Custom(font.Name, (float) font.PointSize);
        }

        public static UIFont ToUIFont(this Font font)
        {
			if (font == null)
				return UIFont.SystemFontOfSize(12);

			var attributes = font.Attributes;
			if (attributes.Name == Device.FontService.SystemFontName)
			{
				var weight = (int) attributes.Weight;
				if (weight > (int)Weight.Regular)
					return UIFont.BoldSystemFontOfSize(attributes.Size);
				
				return UIFont.SystemFontOfSize(attributes.Size);
			}
			
			return UIFont.FromName(attributes.Name, attributes.Size);
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
