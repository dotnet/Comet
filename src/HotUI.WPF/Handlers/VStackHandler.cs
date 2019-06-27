using System.Windows;
using HotUI.Layout;

namespace HotUI.WPF
{
    public class VStackHandler : AbstractLayoutHandler
    {
        public VStackHandler() : base(new VStackLayoutManager<UIElement>())
        {
        }
    }
}