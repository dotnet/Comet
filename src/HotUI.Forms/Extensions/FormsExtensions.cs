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
		public static FPage ToForms (this HotPage hotPage)
		{
			if (hotPage == null)
				return null;
			var handler = hotPage.ViewHandler;
			if(handler == null) {

				handler = Registrar.Pages.GetRenderer (hotPage.GetType ()) as IViewBuilderHandler ;
				hotPage.ViewHandler = handler;
				hotPage.ReBuildView ();
			}
			var page = handler as IFormsPage;
			return page.Page;
		}

		public static FView ToForms (this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null) {

				handler = Registrar.Handlers.GetRenderer (view.GetType ()) as IViewHandler;
				view.ViewHandler = handler;
			}
			var page = handler as IFormsView;
			return page.View;
		}

	}
}