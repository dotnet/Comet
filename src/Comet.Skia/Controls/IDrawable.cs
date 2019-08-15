using SkiaSharp;

namespace Comet.Skia.Controls
{
    public interface IDrawable
    {
        void Draw(SKCanvas canvas, SKSize size);
    }
}
