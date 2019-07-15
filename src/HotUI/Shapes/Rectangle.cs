using HotUI.Graphics;

namespace HotUI
{
    public class Rectangle : Shape
    {
        public override PathF PathForBounds(RectangleF rect)
        {
            var path = new PathF();
            path.AppendRectangle(rect);
            return path;
        }
    }
}