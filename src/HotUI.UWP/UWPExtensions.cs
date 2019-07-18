using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HotUI.UWP
{
    public static class UWPExtensions
    {
        static UWPExtensions()
        {
            UI.Init();
        }

        public static UWPViewHandler GetOrCreateViewHandler(this View view)
        {
            if (view == null)
                return null;
            var handler = view.ViewHandler;
            if (handler == null)
            {
                handler = Registrar.Handlers.GetRenderer(view.GetType());
                view.ViewHandler = handler;
            }

            var iUIElement = handler as UWPViewHandler;
            return iUIElement;
        }

        public static UIElement ToView(this View view)
        {
            var handler = view.GetOrCreateViewHandler();
            return handler?.View;
        }
        
        public static UIElement ToEmbeddableView(this View view)
        {
            var handler = view.GetOrCreateViewHandler();
            if (handler == null)
                throw new Exception("Unable to build handler for view");

            if (handler is FrameworkElement element)
            {
                if (element.Parent is HotUIContainerView container)
                    return container;
            }
            
            return new HotUIContainerView(view);
        }

        public static void RemoveChild(this DependencyObject parent, UIElement child)
        {
            if (parent is Panel panel)
            {
                panel.Children.Remove(child);
                return;
            }

            /*var decorator = parent as Decorator;
            if (decorator != null)
            {
                if (decorator.Child == child)
                {
                    decorator.Child = null;
                }
                return;
            }*/

            if (parent is ContentPresenter contentPresenter)
            {
                if (contentPresenter.Content == child)
                {
                    contentPresenter.Content = null;
                }
                return;
            }

            if (parent is ContentControl contentControl)
            {
                if (contentControl.Content == child)
                {
                    contentControl.Content = null;
                }
                return;
            }
        }
    }
}