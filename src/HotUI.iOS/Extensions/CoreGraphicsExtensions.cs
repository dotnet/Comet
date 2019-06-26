using System.Drawing;
using CoreGraphics;

namespace HotUI.iOS
{
    public static class CoreGraphicsExtensions
    {
        public static SizeF ToSizeF(this CGSize size)
        {
            return new SizeF((float)size.Width, (float)size.Height);
        }
    }
}