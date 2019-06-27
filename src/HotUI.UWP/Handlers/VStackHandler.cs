using Windows.UI.Xaml;
using HotUI.Layout;

namespace HotUI.UWP
{
    public class VStackHandler : AbstractLayoutHandler
    {
        public VStackHandler() : base(new VStackLayoutManager<UIElement>())
        {
        }
    }
}