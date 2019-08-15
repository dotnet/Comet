using SkiaSharp;

namespace Comet.Skia.Controls
{
    public interface IMeasurable
    {
        SKSize Measure(SKSize availableSize);
    }
}
