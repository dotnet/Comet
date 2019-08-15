using Windows.UI.Xaml.Controls;
// ReSharper disable ClassNeverInstantiated.Global
using UWPOrientation = Windows.UI.Xaml.Controls.Orientation;

namespace Comet.UWP.Handlers
{
    public class HStackHandler : AbstractStackLayoutHandler
    {
        
        public HStackHandler()
        {
            Orientation = UWPOrientation.Horizontal;
        }
    }
}