using Windows.ApplicationModel.Core;
using Windows.UI.Core;
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
            HotUI.PerformInvokeOnMainThread = (a) => Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, a);

        }

        public static CoreDispatcher GetDispatcher()
        {
            CoreDispatcher dispatcher = null;

            var coreWindow = CoreWindow.GetForCurrentThread();
            if (coreWindow != null)
                dispatcher = coreWindow.Dispatcher;

            return dispatcher ?? (dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher);
        }

        private static void InvokeWithDispatcher(CoreDispatcher dispatcher, Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch (Exception exc)
                {
                    tcs.SetException(exc);
                }
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            AsyncPump.Run(async delegate { await tcs.Task; });
        }
    }
}