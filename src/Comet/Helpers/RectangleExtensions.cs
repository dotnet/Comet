using System;
using System.Drawing;

namespace Comet
{
    public static class RectangleExtensions
    {
        public static bool BoundsContains(this RectangleF rect, PointF point) =>
              point.X >= 0 && point.X <= rect.Width &&
              point.Y >= 0 && point.Y <= rect.Height;

    }
}
