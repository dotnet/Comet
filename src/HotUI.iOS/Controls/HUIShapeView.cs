using System;
using CoreGraphics;
using UIKit;

namespace HotUI.iOS
{
    public class HUIShapeView : UIView
    {
        public HUIShapeView()
        {
            BackgroundColor = UIColor.Clear;
        }

        public Shape Shape
        {
            get;
            internal set;
        }

        public override void Draw(CGRect rect)
        {
            var context = UIGraphics.GetCurrentContext();
            if (Shape != null)
            { 
                var stroke = Shape.GetStroke(4);
                var color = Shape.GetColor(Color.Black);

                context.SetLineWidth(stroke);
                context.SetStrokeColor(color.ToCGColor());

                if (Shape is Circle circle)
                {
                    var size = Math.Min(rect.Width, rect.Height);
                    size -= stroke;

                    var x = (rect.Width - size) / 2;
                    var y = (rect.Height - size) / 2;

                    var path = new CGPath();
                    path.AddEllipseInRect(new CGRect(x, y, size, size));
                    path.CloseSubpath();

                    context.AddPath(path);
                    context.StrokePath();
                }
            }
        }
    }
}
