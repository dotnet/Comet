using AppKit;

namespace HotUI.Mac.Extensions {
	public static partial class MacExtensions {


		static MacExtensions()
		{
			UI.Init ();
		}
		public static NSViewController ToViewController (this HotPage hotPage)
		{
			if (hotPage == null)
				return null;
			var handler = hotPage.ViewHandler;
			if (handler == null) {

				handler = Registrar.Pages.GetRenderer (hotPage.GetType ()) as IViewBuilderHandler;
				hotPage.ViewHandler = handler;
				hotPage.ReBuildView ();
			}
			var page = handler as INSViewController;
			return page.ViewController;
		}

		public static NSView ToView (this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null) {

				handler = Registrar.Handlers.GetRenderer (view.GetType ()) as IViewHandler;
				view.ViewHandler = handler;
			}
			var page = handler as INSView;
			return page.View;
		}
	}
}
