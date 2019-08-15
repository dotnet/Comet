using Comet.Graphics;
using SkiaSharp;

namespace Comet.Skia
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

        public static SKSize ToSKSize(this SizeF size)
            => new SKSize(size.Width, size.Height);

        public static SizeF ToSizeF(this SKSize size)
            => new SizeF(size.Width, size.Height);

         public static SKPath ToSKPath(
            this PathF path)
        {
            var nativePath = new SKPath();
            
            var pointIndex = 0;
            var arcAngleIndex = 0;
            var arcClockwiseIndex = 0;

            foreach (var operation in path.PathOperations)
            {
                if (operation == PathOperation.MoveTo)
                {
                    var point = path[pointIndex++];
                    nativePath.MoveTo(point.X, point.Y);
                }
                else if (operation == PathOperation.Line)
                {
                    var point = path[pointIndex++];
                    nativePath.LineTo(point.X, point.Y);
                }

                else if (operation == PathOperation.Quad)
                {
                    var controlPoint = path[pointIndex++];
                    var point = path[pointIndex++];
                    nativePath.QuadTo(controlPoint.X, controlPoint.Y, point.X, point.Y);
                }
                else if (operation == PathOperation.Cubic)
                {
                    var controlPoint1 = path[pointIndex++];
                    var controlPoint2 = path[pointIndex++];
                    var point = path[pointIndex++];
                    nativePath.CubicTo(controlPoint1.X, controlPoint1.Y, controlPoint2.X, controlPoint2.Y, point.X,
                        point.Y);
                }
                else if (operation == PathOperation.Arc)
                {
                    var topLeft = path[pointIndex++];
                    var bottomRight = path[pointIndex++];
                    var startAngle = path.GetArcAngle(arcAngleIndex++);
                    var endAngle = path.GetArcAngle(arcAngleIndex++);
                    var clockwise = path.IsArcClockwise(arcClockwiseIndex++);

                    while (startAngle < 0)
                        startAngle += 360;

                    while (endAngle < 0)
                        endAngle += 360;

                    var rect = new SKRect(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
                    var sweep = GraphicsOperations.GetSweep(startAngle, endAngle, clockwise);

                    startAngle *= -1;
                    if (!clockwise)
                        sweep *= -1;

                    nativePath.AddArc(rect, startAngle, sweep);
                }
                else if (operation == PathOperation.Close)
                {
                    nativePath.Close();
                }
            }

            return nativePath;
        }
    }
}