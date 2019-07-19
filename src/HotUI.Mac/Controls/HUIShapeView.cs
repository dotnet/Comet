using System;
using CoreGraphics;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac
{
    public class HUIShapeView : NSView
    {
        public HUIShapeView()
        {

        }

        public Shape Shape
        {
            get;
            internal set;
        }

        public override void DrawRect(CGRect rect)
        {
            var context = NSGraphicsContext.CurrentContext.GraphicsPort;

            if (Shape != null)
            { 
                var stroke = Shape.GetStroke(1);
                var color = Shape.GetColor(Color.Black);

                context.SetLineWidth(stroke);
                context.SetStrokeColor(color.ToCGColor());

                var shapeBounds = new RectangleF(
                    (float)rect.X + (stroke / 2),
                    (float)rect.Y + (stroke / 2),
                    (float)rect.Width - stroke,
                    (float)rect.Height - stroke);

                var path = Shape.PathForBounds(shapeBounds);
                context.AddPath(path.ToCGPath());
                context.StrokePath();
            }
        }
    }
}
