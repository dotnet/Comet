using AppKit;
using CoreGraphics;
using HotUI.Layout;

namespace HotUI.Mac.Handlers
{
    public class HStackHandler : AbstractLayoutHandler
    {
        public HStackHandler(CGRect rect) : base(rect, new HStackLayoutManager<NSView>())
        {
        }

        public HStackHandler() : base(new HStackLayoutManager<NSView>())
        {
        }
    }
}