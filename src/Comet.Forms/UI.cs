using System;
using Comet;
using Comet.Forms.Handlers;

namespace Comet.Forms {
	public static class UI {
		static bool didInit;
		public static void Init ()
		{
			if (didInit)
				return;
			didInit = true;
			Registrar.Handlers.Register<Button, ButtonHandler> ();
			Registrar.Handlers.Register<TextField, TextFieldHandler> ();
			Registrar.Handlers.Register<Text, TextHandler> ();
			Registrar.Handlers.Register<VStack, VStackHandler> ();
			Registrar.Handlers.Register<HStack, HStackHandler> ();
			Registrar.Handlers.Register<WebView, WebViewHandler> ();
			Registrar.Handlers.Register<ScrollView, ScrollViewHandler> ();
			Registrar.Handlers.Register<Image, ImageHandler> ();
			Registrar.Handlers.Register<ListView, ListViewHandler> ();
			Registrar.Handlers.Register<View, ViewHandler> ();
            Registrar.Handlers.Register<ContentView, ContentViewHandler>();
            Registrar.Handlers.Register<Toggle, ToggleHandler>();
            Registrar.Handlers.Register<ViewRepresentable, ViewRepresentableHandler>();

            ModalView.PerformPresent = (o) => Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync (o.ToPage ());
			ModalView.PerformDismiss = () => Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();
			Device.PerformInvokeOnMainThread = Xamarin.Forms.Device.BeginInvokeOnMainThread;
		}
	}
}
