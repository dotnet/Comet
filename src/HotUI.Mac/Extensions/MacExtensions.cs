using AppKit;

namespace HotUI.Mac.Extensions
{
    public static partial class MacExtensions
    {
        static MacExtensions()
        {
            UI.Init();
        }

		public static NSViewController ToViewController (this View view, bool allowNav = true)
		{

			var handler = view.ToINSView ();

			var vc = new HotUIViewController {
				CurrentView = view.ToINSView (),
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

		public static NSView ToView (this View view) => view?.ToINSView ()?.View;

		public static INSView ToINSView(this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null) {
				handler = Registrar.Handlers.GetRenderer (view.GetType ()) as IViewHandler;
				view.ViewHandler = handler;
			}

			return handler as INSView;
		}

	}
}