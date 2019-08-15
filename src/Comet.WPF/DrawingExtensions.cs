using WPFSize = System.Windows.Size;
using WPFRect = System.Windows.Rect;

namespace Comet.WPF
{
    public static class DrawingExtensions
    {
        public static SizeF ToSizeF(this WPFSize size)
        {
            return new SizeF((float)size.Width, (float)size.Height);
        }

        public static WPFSize ToSize(this SizeF size)
        {
            return new WPFSize(size.Width, size.Height);
        }

        public static WPFRect ToRect(this RectangleF rect)
        {
            return new WPFRect(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
