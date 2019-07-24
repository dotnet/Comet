using System;
using SkiaSharp;

namespace HotUI.Skia
{
    public class SkiaShapeView : AbstractControlDelegate
    {
        private Shape _shape;
        public Shape Shape
        {
            get => _shape;
            set
            {
                _shape = value;
            }
        }

        private SKRect _sweekyTest;
        public SKRect SweekyTest
        {
            get => _sweekyTest;
            set
            {
                _sweekyTest = value;
            }
        }

        public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
        {
            canvas.Clear(SKColors.White);
            var paint = new SKPaint{ Color= SKColors.Fuchsia};
            canvas.DrawRect(new SKRect(0, 0, 50, 50), paint);
        }
    }
}
