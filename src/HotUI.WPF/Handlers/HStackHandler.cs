using System.Windows;
using HotUI.Layout;

namespace HotUI.WPF
{
    public class HStackHandler : AbstractLayoutHandler
    {
        
        public HStackHandler() : base(new HStackLayoutManager<UIElement>())
        {
        }
    }
}