using System;
using HotUI.Skia;

namespace HotUI.Samples.Skia
{
    public class SkiaSample5 : View
    {
        [Body]
        View body() => new SkiaShapeView();
    }
}
