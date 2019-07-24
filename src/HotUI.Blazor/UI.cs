using HotUI.Blazor.Handlers;

namespace HotUI.Blazor
{
    public static class UI
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
            Registrar.Handlers.Register<View, ViewHandler>();

            Device.PerformInvokeOnMainThread = a => a();
            ListView.HandlerSupportsVirtualization = false;
        }
    }
}
