using System;
using CoreGraphics;
using AppKit;
using Comet.Mac.Extensions;

namespace Comet.Mac
{
    public class HUIShapeView : NSView
    {
        public View View { get; internal set; }
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
                var stroke = Shape.GetLineWidth(View, 1);
                var color = Shape.GetStrokeColor(View, Color.Black);

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
