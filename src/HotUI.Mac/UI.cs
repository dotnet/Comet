using HotUI.Mac.Handlers;

namespace HotUI.Mac {
	public static class UI {
		static bool hasInit;
		public static void Init ()
		{
			if (hasInit)
				return;
			hasInit = true;
			Registrar.Handlers.Register<Button, ButtonHandler> ();
			Registrar.Handlers.Register<Entry, EntryHandler> ();
			Registrar.Handlers.Register<Label, LabelHandler> ();
			Registrar.Handlers.Register<Stack, StackHandler> ();
			//Registrar.Handlers.Register<WebView, WebViewHandler> ();
			Registrar.Handlers.Register<ScrollView, ScrollViewHandler> ();

			Registrar.Pages.Register<HotPage, HotPageHandler> ();
		}
	}
}
