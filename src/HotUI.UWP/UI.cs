using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using HotUI.UWP.Handlers;
using HotUI.UWP.Services;

namespace HotUI.UWP
{
    public static class UI
    {
        private static bool _hasInitialized;

        public static void Init()
        {
            if (_hasInitialized) return;
            _hasInitialized = true;

            // Controls
            Registrar.Handlers.Register<Button, ButtonHandler>();
            Registrar.Handlers.Register<Image, ImageHandler>();
            Registrar.Handlers.Register<Text, TextHandler>();
            Registrar.Handlers.Register<TextField, TextFieldHandler>();
            Registrar.Handlers.Register<Toggle, ToggleHandler>();
            //Registrar.Handlers.Register<WebView, WebViewHandler> ();

            // Containers
            Registrar.Handlers.Register<ContentView, ContentViewHandler>();
            Registrar.Handlers.Register<ListView, ListViewHandler>();
            Registrar.Handlers.Register<ScrollView, ScrollViewHandler>();
            Registrar.Handlers.Register<View, ViewHandler>();

            // Common Layout
            Registrar.Handlers.Register<Spacer, SpacerHandler>();

            // Native Layout
            //Registrar.Handlers.Register<VStack, VStackHandler>();
            //Registrar.Handlers.Register<HStack, HStackHandler>();

            // Managed Layout
            Registrar.Handlers.Register<VStack, ManagedVStackHandler>();
            Registrar.Handlers.Register<HStack, ManagedHStackHandler>();
            Registrar.Handlers.Register<ZStack, ManagedZStackHandler>();
            Registrar.Handlers.Register<Grid, ManagedGridHandler>();

            Device.PerformInvokeOnMainThread = async a => await GetDispatcher().RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => a());

            Device.BitmapService = new UWPBitmapService();

            ListView.HandlerSupportsVirtualization = false;
        }

        public static CoreDispatcher GetDispatcher()
        {
            CoreDispatcher dispatcher = null;

            var coreWindow = CoreWindow.GetForCurrentThread();
            if (coreWindow != null)
                dispatcher = coreWindow.Dispatcher;

            return dispatcher ?? (dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher);
        }
    }
}