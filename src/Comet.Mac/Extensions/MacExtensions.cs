using AppKit;
using Comet.Internal;
using CoreGraphics;

namespace Comet.Mac.Extensions
{
	public static class MacExtensions
	{
		static MacExtensions()
		{
			UI.Init();
		}

		public static NSViewController ToViewController(this View view, bool allowNav = true)
		{
			var handler = view.GetOrCreateViewHandler();

			var vc = new CometViewController
			{
				CurrentView = view,
			};
			if (view.BuiltView is NavigationView nav && allowNav)
			{
				var navController = new NSNavigationController();
				nav.SetPerformNavigate((toView) => {
					//Since iOS doesn't allow nested navigations, pass the navigate along
					if (toView is NavigationView newNav)
					{
						newNav.SetPerformNavigate(nav);
						newNav.SetPerformPop(nav);
					}
					navController.PushViewController(toView.ToViewController(false), true);
				});
				nav.SetPerformPop(() => navController.Pop());
				navController.PushViewController(vc, false);
				return navController;
			}
			view.ViewHandler = vc;
			return vc;
		}

		public static NSView ToView(this View view) => view?.GetOrCreateViewHandler()?.View;

		public static MacViewHandler GetOrCreateViewHandler(this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null)
			{
				var builtView = view.GetView();
				handler = Registrar.Handlers.GetHandler(builtView.GetType());
				builtView.ViewHandler = handler;
			}

			return handler as MacViewHandler;
		}

		public static FontAttributes ToFont(this NSFont font)
		{
			if (font == null)
				throw new System.ArgumentNullException("Font");

			return new FontAttributes
			{
				Family = font.FamilyName,
				Size = (float)font.PointSize,
			};
		}

		public static NSFont ToNSFont(this FontAttributes attributes)
		{
			if (attributes == null)
				return NSFont.SystemFontOfSize(12);

			if (attributes.Family == Device.FontService.SystemFontName)
			{
				var weight = (int)attributes.Weight;
				if (weight > (int)Weight.Regular)
					return NSFont.BoldSystemFontOfSize(attributes.Size);

				return NSFont.SystemFontOfSize(attributes.Size);
			}

			return NSFont.FromFontName(attributes.Family, attributes.Size);
		}

		public static CGColor ToCGColor(this Color color)
		{
			if (color == null)
				return null;

			return new CGColor(color.R, color.G, color.B, color.A);
		}
	}
}
