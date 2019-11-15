using System;
using SkiaSharp;
using CButton = Comet.Button;
using System.Linq;

namespace Comet.Skia
{
    public class Button : SkiaControl
    {
		static float hPadding = 40;
        static float minHPadding = 10;
		static float vPadding = 10;
        static FontAttributes defaultFont = new FontAttributes
        {
            Family = "System",
            Size = 12,
            Weight = Weight.Bold,
        };
        CButton VirtualButton => VirtualView as CButton;
        public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
        {
			base.Draw(canvas,dirtyRect);
            DrawText(VirtualButton.Text,canvas,VirtualView.GetFont(defaultFont), VirtualView.GetColor(Color.Black),
                VirtualView.GetTextAlignment(TextAlignment.Center)?? TextAlignment.Center,
                VirtualView.GetLineBreakMode(LineBreakMode.NoWrap));
        }

        public override SizeF Measure(SizeF availableSize)
        {
            var button = VirtualButton;
            var lines = SkiaText.GetLines(button.Text, button.GetFont(defaultFont),
                button.GetTextAlignment(TextAlignment.Center) ?? TextAlignment.Center,
                button.GetLineBreakMode(LineBreakMode.NoWrap),availableSize.Width - minHPadding);
            float width = 0;
            float height = 0;
            foreach(var l in lines)
            {
                width = Math.Max(width, l.Width);
                height += l.Height;
            }
            return new SizeF(width + hPadding,height + vPadding);
        }
    }
}
