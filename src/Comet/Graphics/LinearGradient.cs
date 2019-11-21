using System.Drawing;
using System.Net;

namespace Comet.Graphics
{
    public class LinearGradient : Gradient
    {
        public LinearGradient(Color[] colors, PointF startPoint, PointF endPoint) : base(colors)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public LinearGradient(Stop[] stops, PointF startPoint, PointF endPoint) : base(stops)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }
        
        public PointF StartPoint { get; }
        public PointF EndPoint { get; }
    }
}
