using System;
using System.Windows;
using System.Windows.Controls;

namespace Comet.WPF
{
    public static class WPFExtensions
    {
        static WPFExtensions()
        {
            UI.Init();
        }

        public static WPFViewHandler GetOrCreateViewHandler(this View view)
        {
            if (view == null)
                return null;
            var handler = view.ViewHandler;
            if (handler == null)
            {
                handler = Registrar.Handlers.GetHandler(view.GetType());
                view.ViewHandler = handler;
            }

            var iUIElement = handler as WPFViewHandler;
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

            return new CometContainerView(view);
        }

        public static void RemoveChild(this DependencyObject parent, UIElement child)
        {
            if (parent is Panel panel)
            {
                panel.Children.Remove(child);
                return;
            }

            if (parent is Decorator decorator)
            {
                if (decorator.Child == child)
                {
                    decorator.Child = null;
                }
                return;
            }

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