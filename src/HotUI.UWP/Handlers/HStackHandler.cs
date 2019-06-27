using Windows.UI.Xaml;
using HotUI.Layout;

namespace HotUI.UWP
{
    public class HStackHandler : AbstractLayoutHandler
    {
        
        public HStackHandler() : base(new HStackLayoutManager<UIElement>())
        {
        }
    }
}