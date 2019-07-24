using HotUI.Blazor.Handlers;
using Microsoft.AspNetCore.Components.RenderTree;

namespace HotUI.Blazor
{
    internal static class BlazorExtensions
    {
        static BlazorExtensions()
        {
            UI.Init();
        }

        public static IBlazorViewHandler GetOrCreateViewHandler(this View view)
        {
            if (view == null)
            {
                return null;
            }

            var handler = view.ViewHandler;
            if (handler == null)
            {
                handler = Registrar.Handlers.GetHandler(view.GetType());
                view.ViewHandler = handler;
            }

            return handler as IBlazorViewHandler;
        }

    }
}
