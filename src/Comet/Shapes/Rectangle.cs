using Comet.Graphics;
using System.Drawing;

namespace Comet
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
