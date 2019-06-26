using CoreGraphics;
using HotUI.Layout;
using UIKit;

namespace HotUI.iOS
{
    public class VStackHandler : AbstractLayoutHandler
    {
        public VStackHandler(CGRect rect) : base(rect, new VStackLayoutManager<UIView>())
        {    
        }

        public VStackHandler() : base(new VStackLayoutManager<UIView>())
        {
        }
    }
}