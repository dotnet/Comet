using System.Drawing;
using System.Numerics;
using Windows.UI.Input;
using UWPSize = Windows.Foundation.Size;
using UWPRect = Windows.Foundation.Rect;
using UWPPoint = Windows.Foundation.Point;

namespace Comet.UWP
{
    public static class FoundationExtensions
    {
        public static PointF ToPointF(this PointerPoint point)
        {
            return new PointF((float)point.RawPosition.X, (float)point.RawPosition.Y);
        }

        public static SizeF ToSizeF(this Vector2 size)
        {
            return new SizeF((float)size.X, (float)size.Y);
        }

        public static SizeF ToSizeF(this UWPSize size)
        {
            return new SizeF((float) size.Width, (float) size.Height);
        }

        public static UWPSize ToSize(this SizeF size)
        {
            return new UWPSize((float)size.Width, (float)size.Height);
        }

        public static UWPRect ToRect(this RectangleF rect)
        {
            return new UWPRect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static UWPPoint ToPoint(this PointF point)
        {
            return new UWPPoint(point.X, point.Y);
        }
    }
}
