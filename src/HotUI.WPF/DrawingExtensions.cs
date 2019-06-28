using System.Drawing;
using Size = System.Windows.Size;

namespace HotUI.WPF
{
    public static class DrawingExtensions
    {
        public static SizeF ToSizeF(this Size size)
        {
            return new SizeF((float) size.Width, (float) size.Height);
        }
    }
}