using CoreGraphics;

namespace HotUI.Mac
{
    public static class CoreGraphicsExtensions
    {
        public static Size ToHotUISize(this CGSize size)
        {
            return new Size((float)size.Width, (float)size.Height);
        }

        public static CGSize ToCGSize(this Size size)
        {
            return new CGSize(size.Width, size.Height);
        }
    }
}