using System;
using SkiaSharp;
using CButton = Comet.Button;
using System.Linq;
using System.Drawing;

namespace Comet.Skia
{
    public enum ButtonState
    {
        Normal,
        Hover,
        Pressed
    }

    public class ButtonHandler : SkiaControl
    {
		static float hPadding = 40;
        static float minHPadding = 10;
		static float vPadding = 10;

        static FontAttributes defaultFont = new FontAttributes
        {
            Family = "System",
            Size = 14,
            Weight = Weight.Bold,
        };
        CButton VirtualButton => VirtualView as CButton;
        public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
        {
            canvas.Clear(Color.Transparent.ToSKColor());
            var border = VirtualView?.GetBorder();

            var backgroundColor = VirtualView?.GetBackgroundColor();
            if (buttonState != ButtonState.Normal)
                backgroundColor = backgroundColor.WithAlpha(.8f);
            if (border != null)
                DrawBorder(canvas, border, dirtyRect, backgroundColor);
            else
                DrawBackground(canvas, backgroundColor);

            DrawText(VirtualButton.Text,canvas, VirtualView.GetFont(defaultFont), VirtualView.GetColor(Color.Black),
                VirtualView.GetTextAlignment(TextAlignment.Center)?? TextAlignment.Center,
                VirtualView.GetLineBreakMode(LineBreakMode.NoWrap), VerticalAlignment.Center);
        }

        public override SizeF Measure(SizeF availableSize)
        {
            var button = VirtualButton;
            var size = SkiaTextHelper.GetTextSize(button.Text, button.GetFont(defaultFont),
                button.GetTextAlignment(TextAlignment.Center) ?? TextAlignment.Center,
                button.GetLineBreakMode(LineBreakMode.NoWrap),availableSize.Width - minHPadding);
            return new SizeF(size.Width + hPadding,size.Height + vPadding);
        }

        public override bool StartInteraction(PointF[] points)
        {

            ButtonState = ButtonState.Pressed;
            return base.StartInteraction(points);
        }

        public override void HoverInteraction(PointF[] points)
        {
            ButtonState = ButtonState.Hover;
            base.HoverInteraction(points);
        }


        public override void EndInteraction(PointF[] points, bool contained)
        {
            ButtonState = ButtonState.Normal;
            if(contained)
                VirtualButton?.OnClick();
            base.EndInteraction(points,contained);
        }

        public override void CancelInteraction()
        {
            ButtonState = ButtonState.Normal;
            base.CancelInteraction();
        }

        ButtonState buttonState = ButtonState.Normal;
        public ButtonState ButtonState {
            get => buttonState;
            set
            {
                if (buttonState == value)
                    return;
                buttonState = value;
                Invalidate();
            }
        }

        public override string AccessibilityText() => VirtualButton?.Text;
    }
}
