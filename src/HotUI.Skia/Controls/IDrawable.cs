using SkiaSharp;

namespace HotUI.Skia.Controls
{
    public interface IDrawable
    {
        void Draw(SKCanvas canvas, SKSize size);
    }
}
