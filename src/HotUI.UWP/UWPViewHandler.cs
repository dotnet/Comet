using System;
using Windows.UI.Xaml;

namespace HotUI.UWP
{
    public interface UWPViewHandler : IViewHandler
    {
        event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        UIElement View { get; }
    }
}