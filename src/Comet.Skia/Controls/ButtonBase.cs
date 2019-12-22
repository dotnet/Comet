using System;
using System.Drawing;
using System.Linq;
using SkiaSharp;

namespace Comet.Skia.Controls
{
    public class ButtonBase : AbstractControlDelegate
    {
        private ButtonState _state = ButtonState.Normal;
        private IDrawable _normal;
        private IDrawable _pressed;
        private IDrawable _hover;
        private Action _onClick;

        // TODO Add focused, ect
        public ButtonBase(IDrawable normal, IDrawable pressed, IDrawable hover, Action onClick)
        {
            _normal = normal;
            _pressed = pressed;
            _hover = hover;
            _onClick = onClick;
        }

        public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
        {
            canvas.Clear(SKColors.Transparent);

            switch (_state)
            {
                case ButtonState.Normal:
                    _normal.Draw(canvas, Bounds.Size.ToSKSize());
                    break;
                case ButtonState.Pressed:
                    _pressed.Draw(canvas, Bounds.Size.ToSKSize());
                    break;
                case ButtonState.Hover:
                    _hover.Draw(canvas, Bounds.Size.ToSKSize());
                    break;
            }
        }

        public override SizeF Measure(SizeF availableSize)
            => (_normal as IMeasurable)?.Measure(availableSize.ToSKSize()).ToSizeF() ?? availableSize;

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
            _onClick?.Invoke();
        }

        private ButtonState State
        {
            get => _state;
            set
            {
                if (_state != value)
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
