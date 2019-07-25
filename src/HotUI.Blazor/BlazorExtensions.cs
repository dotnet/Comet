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

        /// <summary>
        /// Checks if an internal type of <see cref="View"/> has a defined handler.
        /// </summary>
        public static bool IsIUnsupportednternalView(this View view)
        {
            var handler = view.GetOrCreateViewHandler();

            return handler is ViewHandler && view.GetType().Assembly == typeof(View).Assembly;
        }
    }
}
