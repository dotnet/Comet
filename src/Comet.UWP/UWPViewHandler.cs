using System;
using Windows.UI.Xaml;

namespace Comet.UWP
{
    public interface UWPViewHandler : IViewHandler
    {
        event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        UIElement View { get; }
    }
}
