using System;
using CoreGraphics;
using UIKit;

namespace HotUI.iOS
{
    public class HUIShapeView : UIView
    {
        private Shape _shape;
        
        public HUIShapeView()
        {
            BackgroundColor = UIColor.Clear;
        }

        public Shape Shape
        {
            get => _shape;
            internal set
            {
                _shape = value;
                SetNeedsDisplay();
            }
        }

        public override void Draw(CGRect rect)
        {
            var context = UIGraphics.GetCurrentContext();
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
