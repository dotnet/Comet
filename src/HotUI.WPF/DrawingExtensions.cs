using System.Drawing;
using Size = System.Windows.Size;

namespace HotUI.WPF
{
    public static class DrawingExtensions
    {
        public static Size ToSize(this Size size)
        {
            return new Size((float) size.Width, (float) size.Height);
        }
    }
}