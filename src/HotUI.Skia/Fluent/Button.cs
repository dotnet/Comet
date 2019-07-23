using System;
using System.Linq;
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
    public class Button : AbstractControlDelegate
    {
        private const float Pad = 10;

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

        private SKPaint font = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Typeface = SKFontManager.Default.MatchFamily("Segoe UI", SKFontStyle.Normal),
            TextSize = 14
        };

        private SizeF _textSize;
        private SizeF _size;
        private ButtonState _state;


        // TODO How to bind?
        // TODO How do we support rich text/images
        public Button(string text, Action onClick = null)
        {
            Text = text;
            OnClick = onClick;

            var bounds = new SKRect();
            font.MeasureText(Text, ref bounds);
            _textSize = bounds.Size.ToSizeF();
            _size = new SizeF(_textSize.Width + Pad * 2, _textSize.Height + Pad * 2);
        }

        public string Text { get; }

        public Action OnClick { get; }

        public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
        {
            canvas.Clear(SKColors.Red);

            var background = State == ButtonState.Pressed ? backgroundPressed : backgroundNormal;
            canvas.DrawRect(new SKRect(0, 0, _size.Width, _size.Height), background);

            if (State == ButtonState.Hover)
            {
                canvas.DrawRect(new SKRect(0, 0, _size.Width, _size.Height), borderHover);
            }

            canvas.DrawText(Text, Pad, Pad + _textSize.Height, font);
        }

        public override SizeF Measure(SizeF availableSize) => _size;

        public override void HoverInteraction(PointF[] points)
        {
            // TODO How should we handle multitouch??? Maybe we should be using some sort of gesture thinggy?
            State = points.Any(point => Bounds.Contains(point)) ? ButtonState.Hover : ButtonState.Normal;
            Invalidate();
        }

        public override void EndHoverInteraction()
        {
            State = ButtonState.Normal;
            Invalidate();
        }

        public override bool StartInteraction(PointF[] points)
        {
            State = ButtonState.Pressed;
            Invalidate();
            return true;
        }

        public override void DragInteraction(PointF[] points)
        {
            State = points.Any(point => Bounds.Contains(point)) ? ButtonState.Pressed : ButtonState.Normal;
            Invalidate();
        }

        public override void CancelInteraction()
        {
            State = ButtonState.Normal;
            Invalidate();
        }

        public override void EndInteraction(PointF[] points)
        {
            if (State != ButtonState.Pressed) return;

            State = ButtonState.Normal;
            Invalidate();
            OnClick?.Invoke();
        }

        private ButtonState State
        {
            get => _state;
            set
            {
                if (value != _state)
                {
                    _state = value;
                    Invalidate();
                }
            }
        }

        private enum ButtonState
        {
            Normal,
            Hover,
            Pressed
        }
    }
}
