using AppKit;

namespace HotUI.Mac
{
    public interface INSView : IViewHandler
    {
        NSView View { get; }
    }
}