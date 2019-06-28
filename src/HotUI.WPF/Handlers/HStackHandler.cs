using System.Windows;
using HotUI.Layout;

namespace HotUI.WPF
{
    public class HStackHandler : AbstractStackLayoutHandler
    {        
        public HStackHandler() 
        {
            Orientation = System.Windows.Controls.Orientation.Horizontal;
        }
    }
}