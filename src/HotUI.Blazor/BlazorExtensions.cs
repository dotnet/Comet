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

        public static void AddView(this RenderTreeBuilder builder, int seq, View view)
        {
            builder.AddView(ref seq, view);
        }

        public static void AddView(this RenderTreeBuilder builder, ref int seq, View view)
        {
            var handler = view.GetOrCreateViewHandler();

            builder.OpenComponent(seq++, handler.Component);
            builder.AddComponentReferenceCapture(seq++, handler.SetNativeView);
            builder.CloseComponent();
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
