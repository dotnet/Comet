using Foundation;
using HotUI.Mac.Handlers;

namespace HotUI.Mac
{
    public static class UI
    {

		static NSObject invoker = new NSObject ();
		static bool hasInit;

        public static void Init()
        {
            if (hasInit)
                return;
            hasInit = true;
            Registrar.Handlers.Register<Button, ButtonHandler>();
            Registrar.Handlers.Register<TextField, TextFieldHandler>();
            Registrar.Handlers.Register<Text, TextHandler>();
            Registrar.Handlers.Register<VStack, VStackHandler>();
            //Registrar.Handlers.Register<WebView, WebViewHandler> ();
            Registrar.Handlers.Register<ScrollView, ScrollViewHandler>();
			Registrar.Handlers.Register<Image, ImageHandler> ();
			Registrar.Handlers.Register<View, ViewHandler> ();
			Registrar.Handlers.Register<ContentView, ContentViewHandler> ();
			Registrar.Handlers.Register<ListView, ListViewHandler> ();

			Device.PerformInvokeOnMainThread = invoker.BeginInvokeOnMainThread;
		}
    }
}