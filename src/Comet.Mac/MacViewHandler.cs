using System;
using AppKit;
using Comet.Mac.Controls;

namespace Comet.Mac
{
    public interface MacViewHandler : IViewHandler
    {
        event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        NSView View { get; }
        
        CUIContainerView ContainerView { get; }
    }
}
