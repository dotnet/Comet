using System;
using HotUI;

namespace HotUI.Forms {
	public static class UI {
		public static void Init ()
		{
			Registrar.Handlers.Register<Button, ButtonHandler> ();
			Registrar.Handlers.Register<Entry, EntryHandler> ();
			Registrar.Handlers.Register<Label, LabelHandler> ();
			Registrar.Handlers.Register<Stack, StackHandler> ();
			Registrar.Handlers.Register<WebView, WebViewHandler> ();

			Registrar.Pages.Register<HotPage, HotPageHandler> ();
		}
	}
}
