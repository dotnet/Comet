using HotUI.Graphics;

namespace HotUI
{
    public class Ellipse : Shape
    {
        public override PathF PathForBounds(RectangleF rect)
        {
            var path = new PathF();
            path.AppendEllipse(rect);
            return path;
        }
    }
}