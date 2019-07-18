using System;
using System.Windows;

namespace HotUI.WPF
{
    public interface WPFViewHandler : IViewHandler
    {
        event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        UIElement View { get; }
    }
}