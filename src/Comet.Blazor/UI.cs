using Comet.Blazor.Handlers;

namespace Comet.Blazor
{
    internal static class UI
    {
        private static bool _hasInitialized;

        public static void Init()
        {
            if (_hasInitialized)
            {
                return;
            }

            _hasInitialized = true;

            Registrar.Handlers.Register<Text, TextHandler>();
            Registrar.Handlers.Register<ContainerView, ContainerViewHandler>();
            Registrar.Handlers.Register<Button, ButtonHandler>();
            Registrar.Handlers.Register<ContentView, ContentViewHandler>();
            Registrar.Handlers.Register<View, ViewHandler>();
            Registrar.Handlers.Register<ListView, ListViewHandler>();
            Registrar.Handlers.Register<Spacer, SpacerHandler>();
            Registrar.Handlers.Register<TextField, TextFieldHandler>();
            Registrar.Handlers.Register<ProgressBar, ProgressBarHandler>();
            Registrar.Handlers.Register<Image, ImageHandler>();
            Registrar.Handlers.Register<ScrollView, ScrollViewHandler>();
            Registrar.Handlers.Register<TabView, TabViewHandler>();
            Registrar.Handlers.Register<ShapeView, ShapeViewHandler>();

            // Unsupported views. Without registering these, it cause an infinite recursion on derived views
            Registrar.Handlers.Register<ActivityIndicator, UnsupportedHandler<ActivityIndicator>>();
            //Registrar.Handlers.Register<Control, UnsupportedHandler<Control>>();

            Device.PerformInvokeOnMainThread = a => a();
            ListView.HandlerSupportsVirtualization = false;
        }
    }
}
