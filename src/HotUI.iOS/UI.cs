using System;
using HotUI;

namespace HotUI.iOS {
	public static class UI {
		static bool hasInit;
		public static void Init ()
		{
			if (hasInit)
				return;
			hasInit = true;
			Registrar.Handlers.Register<Button, ButtonHandler> ();
			Registrar.Handlers.Register<TextField, TextFieldHandler> ();
			Registrar.Handlers.Register<Text, TextHandler> ();
			Registrar.Handlers.Register<Stack, StackHandler> ();
			//Registrar.Handlers.Register<WebView, WebViewHandler> ();
			Registrar.Handlers.Register<ScrollView, ScrollViewHandler> ();

			Registrar.Pages.Register<HotPage, HotPageHandler> ();
		}
	}
}
