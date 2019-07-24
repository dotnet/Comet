using SkiaSharp;

namespace HotUI.Skia.Controls
{
    public interface IMeasurable
    {
        SKSize Measure(SKSize availableSize);
    }
}
