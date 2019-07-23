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
            set
            {
                _shape = value;
                SetNeedsDisplay();
            }
        }

        public View View { get; set;}

        public override void Draw(CGRect rect)
        {
            var context = UIGraphics.GetCurrentContext();
            if (Shape != null)
            { 
                var stroke = Shape.GetStroke(View,1);
                var color = Shape.GetColor(View,Color.Black);

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
