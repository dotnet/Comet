using System;
using Comet.Skia.Internal;
using SkiaSharp;
using TextBlock = Topten.RichTextKit.TextBlock;

namespace Comet.Skia
{
    public abstract class SkiaControl : SkiaView
    {
        protected View VirtualView { get; private set; }

        public override SizeF Measure(SizeF availableSize) => new SizeF(100, 44);

        public virtual void SetView(View view)
        {
            VirtualView = view;
        }

        public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
        {
            canvas.Clear(Color.Transparent.ToSKColor());
            if (VirtualView == null)
                return;
            var border = VirtualView?.GetBorder();
            var backgroundColor = VirtualView?.GetBackgroundColor();
            if (border != null)
                DrawBorder(canvas, border, dirtyRect, backgroundColor);
            else
                DrawBackground(canvas, backgroundColor);
        }

        protected virtual void DrawBorder(SKCanvas canvas, Shape shape, RectangleF rect, Color backgroundColor)
        {
            var strokeColor = shape.GetStrokeColor(VirtualView, Color.Black);
            var strokeWidth = shape.GetLineWidth(VirtualView, 1);


            canvas.DrawShape(shape, rect, strokeColor: strokeColor, strokeWidth: strokeWidth, fill: backgroundColor);
        }

        protected virtual void DrawBackground(SKCanvas canvas, Color backgroundColor)
        {
            var paint = new SKPaint();
            paint.Color = backgroundColor.ToSKColor();
            var bounds = new SKRect(0, 0, VirtualView.Frame.Width, VirtualView.Frame.Height);
            canvas.DrawRect(bounds, paint);
        }

        protected void DrawText(string text, SKCanvas canvas, FontAttributes data, Color color, TextAlignment alignment, LineBreakMode lineBreakMode )
        {
            canvas.Save();

            var emojiChar = 0x1F680;

            // ask the font manager for a font with that character
            var fontManager = SKFontManager.Default;
            var emojiTypeface = fontManager.MatchCharacter(emojiChar);

            var paint = new SKPaint
            {
                Color = color.ToSKColor(),
                IsAntialias = true,
                TextSize = data.Size,
                Typeface = emojiTypeface
            };

            //canvas.ClipRect(VirtualView.Frame.ToSKRect());

            var tb = new TextBlock();
            tb.AddText(text, data.ToStyle(color));
            tb.Alignment = alignment.ToTextAlignment();
            tb.MaxWidth = VirtualView.Frame.Width;
            tb.Layout();
            tb.Paint(canvas);
            canvas.Restore();
        }
    }
}
