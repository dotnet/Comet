using Android.Graphics;
using Comet.Graphics;
using APath = Android.Graphics.Path;

namespace Comet.Androids
{
    public static class GraphicsExtensions
    {
        public static APath AsAndroidPath(this PathF path)
        {
            var nativePath = new APath();
            
            int pointIndex = 0;
            int arcAngleIndex = 0;
            int arcClockwiseIndex = 0;

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
                    {
                        startAngle += 360;
                    }

                    while (endAngle < 0)
                    {
                        endAngle += 360;
                    }

                    var rect = new RectF(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
                    var sweep = GraphicsOperations.GetSweep(startAngle, endAngle, clockwise);

                    startAngle *= -1;
                    if (!clockwise)
                    {
                        sweep *= -1;
                    }

                    nativePath.ArcTo(rect, startAngle, sweep);
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