using System.Drawing;
using Size = Windows.Foundation.Size;

namespace HotUI.UWP
{
    public static class FoundationExtensions
    {
        public static SizeF ToSizeF(this Size size)
        {
            return new SizeF((float) size.Width, (float) size.Height);
        }
    }
}