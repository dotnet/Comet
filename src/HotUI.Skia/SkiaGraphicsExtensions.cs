using SkiaSharp;

namespace HotUI.Skia
{
    public static class SkiaGraphicsExtensions
    {
        public static SKColor ToSKColor(this Color target)
        {
            var r = (byte) (target.R * 255f);
            var g = (byte) (target.G * 255f);
            var b = (byte) (target.B * 255f);
            var a = (byte) (target.A * 255f);
            return new SKColor(r, g, b, a);
        }

        public static SKSize ToSKSize(this SizeF size)
            => new SKSize(size.Width, size.Height);

        public static SizeF ToSizeF(this SKSize size)
            => new SizeF(size.Width, size.Height);
    }
}