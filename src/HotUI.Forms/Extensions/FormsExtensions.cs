using System;
using FImage = Xamarin.Forms.Image;
using FButton = Xamarin.Forms.Button;
using FLabel = Xamarin.Forms.Label;
using FStack = Xamarin.Forms.StackLayout;
using FEntry = Xamarin.Forms.Entry;
using FWebview = Xamarin.Forms.WebView;
using FView = Xamarin.Forms.View;
using FPage = Xamarin.Forms.Page;
using HotUI;

namespace HotUI.Forms {
	public static class FormsExtensions {
		static FormsExtensions()
		{
			UI.Init ();
		}
		public static FPage ToPage (this View view, bool allowNav = true)
		{
			var handler = view.GetOrCreateViewHandler ();

			var vc = new HotUIPage {
				Content = new HotUIContainerView(view),
			};
			if (view.BuiltView is NavigationView nav && allowNav) {
				var navController = new Xamarin.Forms.NavigationPage (vc);
				nav.PerformNavigate = (toView) => {
					//Since iOS doesn't allow nested navigations, pass the navigate along
					if (toView is NavigationView newNav) {
						newNav.PerformNavigate = nav.PerformNavigate;
					}
					navController.PushAsync(toView.ToPage (false), true);
				};
				return navController;
			}
			return vc;
		}

		public static FView ToForms (this View view) => view.GetOrCreateViewHandler ()?.View;

		public static FormsViewHandler GetOrCreateViewHandler(this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null) {

				handler = Registrar.Handlers.GetRenderer (view.GetType ()) as IViewHandler;
				view.ViewHandler = handler;
			}
			return handler as FormsViewHandler;
		}

	}
}