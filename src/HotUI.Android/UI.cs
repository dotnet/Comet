using HotUI.Android.Controls;
using HotUI.Android.Handlers;
using HotUI.Android.Services;

namespace HotUI.Android
{
    public static class UI
    {
        static bool _hasInitialized;

        public static void Init()
        {
            if (_hasInitialized) return;
            _hasInitialized = true;

            // Controls
            Registrar.Handlers.Register<Button, ButtonHandler>();
            Registrar.Handlers.Register<Image, ImageHandler>();
            Registrar.Handlers.Register<Slider, SliderHandler>();
            Registrar.Handlers.Register<TextField, TextFieldHandler>();
            Registrar.Handlers.Register<Text, TextHandler>();
            Registrar.Handlers.Register<Toggle, ToggleHandler>();
            //Registrar.Handlers.Register<WebView, WebViewHandler> ();

            // Containers
            Registrar.Handlers.Register<ScrollView, ScrollViewHandler>();
			Registrar.Handlers.Register<ListView, ListViewHandler> ();
			Registrar.Handlers.Register<View, ViewHandler>();
            Registrar.Handlers.Register<ContentView, ContentViewHandler>();
            Registrar.Handlers.Register<ViewRepresentable, ViewRepresentableHandler>();

            // Native Layouts
            Registrar.Handlers.Register<HStack, HStackHandler>();
            Registrar.Handlers.Register<VStack, VStackHandler>();

            // Managed Layouts
            //Registrar.Handlers.Register<HStack, ManagedHStackHandler>();
            //Registrar.Handlers.Register<VStack, ManagedVStackHandler>();
            //Registrar.Handlers.Register<ZStack, ManagedZStackHandler>();
            //Registrar.Handlers.Register<Grid, ManagedGridHandler>();

            // Modal SUpport
            ModalView.PerformPresent = ModalManager.ShowModal;
            ModalView.PerformDismiss = ModalManager.DismisModal;

            // Device Services
            Device.PerformInvokeOnMainThread = (a) => AndroidContext.CurrentContext.RunOnUiThread(a);
            Device.GraphicsService = new AndroidGraphicsService();
            Device.BitmapService = new AndroidBitmapService();
        }
    }
}