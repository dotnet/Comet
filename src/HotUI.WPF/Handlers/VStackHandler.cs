using System.Windows;
using HotUI.Layout;

namespace HotUI.WPF
{
    public class VStackHandler : AbstractStackLayoutHandler
    {
        public VStackHandler()
        {
            Orientation = System.Windows.Controls.Orientation.Vertical;
        }
    }
}