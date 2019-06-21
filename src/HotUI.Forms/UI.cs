using System;
using HotUI;

namespace HotUI.Forms {
	public static class UI {
		static bool didInit;
		public static void Init ()
		{
			if (didInit)
				return;
			didInit = true;
			Registrar.Handlers.Register<Button, ButtonHandler> ();
			Registrar.Handlers.Register<Entry, EntryHandler> ();
			Registrar.Handlers.Register<Label, LabelHandler> ();
			Registrar.Handlers.Register<Stack, StackHandler> ();
			Registrar.Handlers.Register<WebView, WebViewHandler> ();
			Registrar.Handlers.Register<ScrollView, ScrollViewHandler> ();

			Registrar.Pages.Register<HotPage, HotPageHandler> ();
		}
	}
}
