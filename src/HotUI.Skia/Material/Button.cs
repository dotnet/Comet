using System;
using HotUI.Skia.Controls;
using SkiaSharp;

namespace HotUI.Skia.Material
{
    public class Button : ButtonBase
    {
        public Button(string text) : base(null, null, null, null)
        {
        }

        private class ButtonContent : IDrawable
        {
            public ButtonContent(string text)
            {

            }

            public void Draw(SKCanvas canvas, SKSize size)
            {
                // TODO Adapt "material" button code from Xamarin forms skia render

                ////-----------------------------------------------------------------------------
                //        // Draw Group shape group
                //        // Shadow color for RoundRectangleStyleFill
                //var RoundRectangleStyleFillShadowColor = new SKColor(0, 0, 0, 20);

                //// Build shadow for RoundRectangleStyleFill
                //var RoundRectangleStyleFillShadow = SKImageFilter.CreateDropShadow(0, 0, 4, 4, RoundRectangleStyleFillShadowColor, SKDropShadowImageFilterShadowMode.DrawShadowAndForeground, null, null);

                //// Fill color for Round Rectangle Style
                //var RoundRectangleStyleFillColor = button.BackgroundColor.ToSKColor(Color.Transparent);

                //// New Round Rectangle Style fill paint
                //var RoundRectangleStyleFillPaint = new SKPaint()
                //{
                //    Style = SKPaintStyle.Fill,
                //    Color = RoundRectangleStyleFillColor,
                //    BlendMode = SKBlendMode.SrcOver,
                //    IsAntialias = true,
                //    ImageFilter = RoundRectangleStyleFillShadow
                //};

                //// Frame color for Round Rectangle Style
                //var RoundRectangleStyleFrameColor = button.BorderColor.ToSKColor();

                //// New Round Rectangle Style frame paint
                //var RoundRectangleStyleFramePaint = new SKPaint()
                //{
                //    Style = SKPaintStyle.Stroke,
                //    Color = RoundRectangleStyleFrameColor,
                //    BlendMode = SKBlendMode.SrcOver,
                //    IsAntialias = true,
                //    StrokeWidth = (float)button.BorderWidth,
                //};

                //float rounding = (float)button.CornerRadius;
                //if (rounding < 0)
                //    rounding = 0;

                //// Draw Round Rectangle shape
                //var bounds = button.Bounds.ToSKRect(false);
                //bounds.Inflate(-2f, -2f);

                //canvas.DrawRoundRect(bounds, rounding, rounding, RoundRectangleStyleFillPaint);
                //canvas.DrawRoundRect(bounds, rounding, rounding, RoundRectangleStyleFramePaint);

                //DrawText(button.Text, canvas, new TextDrawingData(button));
            }
        }
    }
}
