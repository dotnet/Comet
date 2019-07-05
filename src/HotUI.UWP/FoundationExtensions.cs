using System.Drawing;
using Size = Windows.Foundation.Size;

namespace HotUI.UWP
{
    public static class FoundationExtensions
    {
        public static Size ToSize(this Size size)
        {
            return new Size((float) size.Width, (float) size.Height);
        }
    }
}