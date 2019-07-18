using System.Drawing;
using System.Numerics;
using UWPSize = Windows.Foundation.Size;

namespace HotUI.UWP
{
    public static class FoundationExtensions
    {
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
    }
}