using HotUI.Graphics;
using SkiaSharp;

namespace HotUI.Skia
{
    public static class SkiaGraphicsExtensions
    {
        public static SKColor ToSKColor(this Color target)
        {
            var r = (byte) (target.R * 255f);
            var g = (byte) (target.G * 255f);
            var b = (byte) (target.B * 255f);
            var a = (byte) (target.A * 255f);
            return new SKColor(r, g, b, a);
        }

        public static SizeF ToSizeF(this SKSize size)
            => new SizeF(size.Width, size.Height);

        public static SKPath ToSKPath(this PathF target)
        {
            var path = new SKPath();

            int pointIndex = 0;
            int arcAngleIndex = 0;
            int arcClockwiseIndex = 0;

            foreach(var operation in target.PathOperations)
            {
                if(operation == PathOperation.MoveTo)
                {
                    var point = target[pointIndex++];
                    path.MoveTo(point.X, point.Y);
                }
                else if(operation == PathOperation.Line)
                {
                    var endPoint = target[pointIndex++];
                    path.LineTo(endPoint.X, endPoint.Y);
                }
                else if(operation == PathOperation.Quad)
                {
                    var controlPoint = target[pointIndex++];
                    var endPoint = target[pointIndex++];
                    path.QuadTo(controlPoint.X, controlPoint.Y, endPoint.X, endPoint.Y);
                }
                else if(operation == PathOperation.Cubic)
                {
                    var controlPoint1 = target[pointIndex++];
                    var controlPoint2 = target[pointIndex++];
                    var endpoint = target[pointIndex++];
                    path.CubicTo(
                        controlPoint1.X,
                        controlPoint1.Y,
                        controlPoint2.X,
                        controlPoint2.Y,
                        endpoint.X,
                        endpoint.Y
                        );
                }
                else if(operation == PathOperation.Arc)
                {
                    var topLeft = target[pointIndex++];
                    var bottomRight = target[pointIndex++];
                    float startAngle = target.GetArcAngle(arcAngleIndex++);
                    float endAngle = target.GetArcAngle(arcAngleIndex++);
                    var clockwise = target.IsArcClockwise(arcClockwiseIndex++);

                    //error math
                    while (startAngle < 0)
                    {
                        startAngle += 360;
                    }

                     while(endAngle < 0)
                    {
                        endAngle += 360;
                    }

                    // sweep, in degrees. Positive is clockwise; Negative for anticlockwise
                    var sweepAngle = GraphicsOperations.GetSweep(startAngle, endAngle, clockwise);

                    startAngle *= -1;
                    if (!clockwise)
                        sweepAngle *= -1;

                    path.ArcTo(new SKRect(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y), startAngle, sweepAngle, false);

                }
                else if(operation == PathOperation.Close)
                {
                    path.Close();
                }
            }

            return path;
        }
    }
}