using AppKit;
using CoreGraphics;

namespace HotUI.Mac.Extensions
{
    public static class MacExtensions
    {
        static MacExtensions()
        {
            UI.Init();
        }

		public static NSViewController ToViewController (this View view, bool allowNav = true)
		{
			var handler = view.GetOrCreateViewHandler ();

			var vc = new HotUIViewController 
			{
				CurrentView = view,
			};
			if (view.BuiltView is NavigationView nav && allowNav) {
				var navController = new NSNavigationController ();
				nav.PerformNavigate = (toView) => {
					//Since iOS doesn't allow nested navigations, pass the navigate along
					if (toView is NavigationView newNav) {
						newNav.PerformNavigate = nav.PerformNavigate;
					}
					navController.PushViewController (toView.ToViewController (false), true);
				};
				navController.PushViewController (vc, false);
				return navController;
			}
			return vc;
		}

		public static NSView ToView (this View view) => view?.GetOrCreateViewHandler ()?.View;

		public static MacViewHandler GetOrCreateViewHandler(this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null) {
				handler = Registrar.Handlers.GetHandler(view.GetType ()) as IViewHandler;
				view.ViewHandler = handler;
			}

			return handler as MacViewHandler;
		}

        public static Font ToFont(this NSFont font)
        {
            if (font == null)
                return Font.System(12);

            // todo: implement support for attributes other than name and size.
            return font.FamilyName == Device.FontService.SystemFontName
                ? Font.System((float)font.PointSize)
                : Font.Custom(font.FamilyName, (float)font.PointSize);
        }

        public static NSFont ToNSFont(this Font font)
        {
            if (font == null)
                return NSFont.SystemFontOfSize(12);

            var attributes = font.Attributes;
            if (attributes.Name == Device.FontService.SystemFontName)
            {
                var weight = (int)attributes.Weight;
                if (weight > (int)Weight.Regular)
                    return NSFont.BoldSystemFontOfSize(attributes.Size);

                return NSFont.SystemFontOfSize(attributes.Size);
            }

            return NSFont.FromFontName(attributes.Name, attributes.Size);
        }
        
        public static CGColor ToCGColor(this Color color)
        {
	        if (color == null)
		        return null;

	        return new CGColor(color.R, color.G, color.B, color.A);
        }
    }
}