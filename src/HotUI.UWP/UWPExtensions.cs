using System;
using Windows.UI.Xaml;

namespace HotUI.UWP
{
    public static class UWPExtensions
    {
        static UWPExtensions()
        {
            UI.Init();
        }

        public static IUIElement ToIUIElement(this View view)
        {
            if (view == null)
                return null;
            var handler = view.ViewHandler;
            if (handler == null)
            {
                handler = Registrar.Handlers.GetRenderer(view.GetType());
                view.ViewHandler = handler;
            }

            var iUIElement = handler as IUIElement;
            return iUIElement;
        }

        public static UIElement ToView(this View view)
        {
            var handler = view.ToIUIElement();
            return handler?.View;
        }
        
        public static UIElement ToEmbeddableView(this View view)
        {
            var handler = view.ToIUIElement();
            if (handler == null)
                throw new Exception("Unable to build handler for view");

            return new HotUIContainerView(view);
        }
    }
}