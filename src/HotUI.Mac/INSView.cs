using AppKit;

namespace HotUI.Mac
{
    public interface INSView : IViewHandler
    {
        NSView View { get; }
    }

    public interface INSViewController : IViewBuilderHandler
    {
        NSViewController ViewController { get; }
    }
}