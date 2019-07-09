using System.Drawing;
using WPFSize = System.Windows.Size;

namespace HotUI.WPF
{
    public static class DrawingExtensions
    {
        public static Size ToSize(this WPFSize size)
        {
            return new Size((float)size.Width, (float)size.Height);
        }

        public static WPFSize ToSize(this Size size)
        {
            return new WPFSize((float)size.Width, (float)size.Height);
        }
    }
}