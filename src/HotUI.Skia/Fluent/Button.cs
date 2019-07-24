using System;
using System.Linq;
using HotUI.Skia.Controls;
using SkiaSharp;

namespace HotUI.Skia.Fluent
{
    /// <summary>
    /// Initial "Fluent" button.
    /// 
    /// Lots of things are wrong.
    /// 1. No Accessibility support :'(
    /// 2. State is in the wrong spot. Should be persisted when the control is "rebuilt".
    /// 3. Not in the tab order
    /// 4. Dissapears in Uwp??
    /// 5. ButtonState handling could be improved
    /// 6. Themeing/styling...
    /// 7. Localization (RTL)
    /// 8. Images? Rich text? How do we get these accessible?
    /// 9. Integrate with the Binding system
    /// </summary>
    public class Button : ButtonBase
    {
        private static SKPaint backgroundNormal = new SKPaint
        {
            Color = new SKColor(204, 204, 204)
        };

        private static SKPaint backgroundPressed = new SKPaint
        {
            Color = new SKColor(153, 153, 153)
        };

        private static SKPaint borderHover = new SKPaint
        {
            Color = new SKColor(122, 122, 122),
            IsStroke = true,
            StrokeWidth = 5
        };

        private static SKPaint font = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Typeface = SKFontManager.Default.MatchFamily("Segoe UI", SKFontStyle.Normal),
            TextSize = 14
        };

        public Button(string text, Action onClick = null) :
            base(
                normal: new ButtonContent(text, backgroundNormal, font),
                pressed: new ButtonContent(text, backgroundPressed, font),
                hover: new ButtonContent(text, backgroundNormal, font, borderHover),
                onClick: onClick)
        {
        }


        private class ButtonContent : IDrawable, IMeasurable
        {
            private const float Pad = 10;
            private string _text;
            private SKPaint _background;
            private SKPaint _forground;
            private SKPaint _border;
            private SKSize _textSize;

            public ButtonContent(string text, SKPaint background, SKPaint forground, SKPaint border = null)
            {
                _text = text;
                _background = background;
                _forground = forground;
                _border = border;

                var bounds = new SKRect();
                _forground.MeasureText(_text, ref bounds);
                _textSize = bounds.Size;
            }

            public void Draw(SKCanvas canvas, SKSize size)
            {
                canvas.DrawRect(0, 0, size.Width, size.Height, _background);
                if (_border != null)
                {
                    canvas.DrawRect(0, 0, size.Width, size.Height, _border);
                }

                canvas.DrawText(_text, Pad, Pad + _textSize.Height, _forground);
            }

            public SKSize Measure(SKSize availableSize)
                => new SKSize(_textSize.Width + Pad * 2, _textSize.Height + Pad * 2);
        }
    }
}
