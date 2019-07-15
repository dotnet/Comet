using System.Drawing;
using WPFSize = System.Windows.Size;

namespace HotUI.WPF
{
    public static class DrawingExtensions
    {
        public static SizeF ToSize(this WPFSize size)
        {
            return new SizeF((float)size.Width, (float)size.Height);
        }

        public static WPFSize ToSize(this SizeF size)
        {
            return new WPFSize((float)size.Width, (float)size.Height);
        }
    }
}