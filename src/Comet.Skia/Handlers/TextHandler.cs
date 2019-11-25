using System;
using System.Drawing;
using SkiaSharp;

namespace Comet.Skia
{
    public class TextHandler : SkiaControl
    {
		static float hPadding = 40;
        static float minHPadding = 10;
        static float vPadding = 10;

        static FontAttributes defaultFont = new FontAttributes
        {
            Family = "System",
            Size = 14,
            Weight = Weight.Regular,
        };

        Text VirtualText => (Text)VirtualView;
        public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
        {
            base.Draw(canvas, dirtyRect);

            DrawText(VirtualText.Value, canvas, VirtualView.GetFont(defaultFont), VirtualView.GetColor(Color.Black),
                VirtualView.GetTextAlignment(TextAlignment.Center) ?? TextAlignment.Center,
                VirtualView.GetLineBreakMode(LineBreakMode.NoWrap), VerticalAlignment.Center);
        }

        public override SizeF Measure(SizeF availableSize)
        {
            var text = VirtualText;
            var size = SkiaTextHelper.GetTextSize(text.Value, text.GetFont(defaultFont),
                text.GetTextAlignment(TextAlignment.Center) ?? TextAlignment.Center,
                text.GetLineBreakMode(LineBreakMode.NoWrap), availableSize.Width - minHPadding);
            return new SizeF(size.Width + hPadding, size.Height + vPadding);
        }

        protected override string AccessibilityText() => VirtualText?.Value;
    }
}
