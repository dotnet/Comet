using System;
using AppKit;
using HotUI.Mac.Controls;

namespace HotUI.Mac
{
    public interface MacViewHandler : IViewHandler
    {
        event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        NSView View { get; }
        
        HUIContainerView ContainerView { get; }
    }
}