using CoreGraphics;
using HotUI.Layout;
using UIKit;

namespace HotUI.iOS
{
    public class HStackHandler : AbstractLayoutHandler
    {
        public HStackHandler(CGRect rect) : base(rect, new HStackLayoutManager<UIView>())
        {    
        }

        public HStackHandler() : base(new HStackLayoutManager<UIView>())
        {
        }
    }
}