using HotUI.Blazor.Handlers;

namespace HotUI.Blazor
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

            Device.PerformInvokeOnMainThread = a => a();
            ListView.HandlerSupportsVirtualization = false;
        }
    }
}
