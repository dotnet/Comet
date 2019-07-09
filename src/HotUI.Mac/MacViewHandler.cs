using AppKit;
using HotUI.Mac.Controls;

namespace HotUI.Mac
{
    public interface MacViewHandler : IViewHandler
    {
        NSView View { get; }
        HUIContainerView ContainerView { get; }
    }
}