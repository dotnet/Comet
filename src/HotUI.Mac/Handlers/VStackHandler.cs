using AppKit;
using CoreGraphics;
using HotUI.Layout;

namespace HotUI.Mac.Handlers
{
    public class VStackHandler : AbstractLayoutHandler
    {
        public VStackHandler(CGRect rect) : base(rect, new VStackLayoutManager<NSView>())
        {
        }

        public VStackHandler() : base(new VStackLayoutManager<NSView>())
        {
        }
    }
}