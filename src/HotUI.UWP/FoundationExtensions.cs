using System.Drawing;
using System.Numerics;
using UWPSize = Windows.Foundation.Size;

namespace HotUI.UWP
{
    public static class FoundationExtensions
    {
        public static Size ToSize(this Vector2 size)
        {
            return new Size((float)size.X, (float)size.Y);
        }

        public static Size ToSize(this UWPSize size)
        {
            return new Size((float) size.Width, (float) size.Height);
        }

        public static UWPSize ToSize(this Size size)
        {
            return new UWPSize((float)size.Width, (float)size.Height);
        }
    }
}