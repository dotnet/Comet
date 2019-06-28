using HotUI.UWP.Handlers;

namespace HotUI.UWP
{
    public static class UI
    {
        private static bool hasInit;

        public static void Init()
        {
            if (hasInit)
                return;
            hasInit = true;
            Registrar.Handlers.Register<Button, ButtonHandler>();
            Registrar.Handlers.Register<TextField, TextFieldHandler>();
            Registrar.Handlers.Register<Text, TextHandler>();
            Registrar.Handlers.Register<VStack, VStackHandler>();
            Registrar.Handlers.Register<HStack, HStackHandler>();
            //Registrar.Handlers.Register<WebView, WebViewHandler> ();
            Registrar.Handlers.Register<ScrollView, ScrollViewHandler>();
            Registrar.Handlers.Register<Image, ImageHandler>();
            Registrar.Handlers.Register<ListView, ListViewHandler>();
            Registrar.Handlers.Register<View, ViewHandler>();
        }
    }
}