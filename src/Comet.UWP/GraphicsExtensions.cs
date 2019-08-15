using Comet.Graphics;
using System;
using Windows.UI.Xaml.Media;
using UWPSize = Windows.Foundation.Size;
using UWPColor = Windows.UI.Color;

namespace Comet.UWP
{
    public static class GraphicsExtensions
    { 
        public static UWPColor ToColor(this Color target)
        {
            return UWPColor.FromArgb(
                (byte)(255 * target.A),
                (byte)(255 * target.R),
                (byte)(255 * target.G),
                (byte)(255 * target.B));
        }

        public static PathGeometry AsPathGeometry(this PathF target)
        {
            var geometry = new PathGeometry();
            PathFigure figure = null;

            var pointIndex = 0;
            var arcAngleIndex = 0;
            var arcClockwiseIndex = 0;

            foreach (var operation in target.PathOperations)
            {
                if (operation == PathOperation.MoveTo)
                {
                    figure = new PathFigure();
                    geometry.Figures.Add(figure);
                    figure.StartPoint = target[pointIndex++].ToPoint();
                }
                else if (operation == PathOperation.Line)
                {
                    var lineSegment = new LineSegment { Point = target[pointIndex++].ToPoint() };
                    figure.Segments.Add(lineSegment);
                }
                else if (operation == PathOperation.Quad)
                {
                    var quadSegment = new QuadraticBezierSegment
                    {
                        Point1 = target[pointIndex++].ToPoint(),
                        Point2 = target[pointIndex++].ToPoint()
                    };
                    figure.Segments.Add(quadSegment);
                }
                else if (operation == PathOperation.Cubic)
                {
                    var cubicSegment = new BezierSegment()
                    {
                        Point1 = target[pointIndex++].ToPoint(),
                        Point2 = target[pointIndex++].ToPoint(),
                        Point3 = target[pointIndex++].ToPoint(),
                    };
                    figure.Segments.Add(cubicSegment);
                }
                else if (operation == PathOperation.Arc)
                {
                    var topLeft = target[pointIndex++];
                    var bottomRight = target[pointIndex++];
                    var startAngle = target.GetArcAngle(arcAngleIndex++);
                    var endAngle = target.GetArcAngle(arcAngleIndex++);
                    var clockwise = target.IsArcClockwise(arcClockwiseIndex++);

                    while (startAngle < 0)
                    {
                        startAngle += 360;
                    }

                    while (endAngle < 0)
                    {
                        endAngle += 360;
                    }

                    var sweep = GraphicsOperations.GetSweep(startAngle, endAngle, clockwise);
                    var absSweep = Math.Abs(sweep);

                    var rectX = topLeft.X;
                    var rectY = topLeft.Y;
                    var rectWidth = bottomRight.X - topLeft.X;
                    var rectHeight = bottomRight.Y  - topLeft.Y;

                    var startPoint = GraphicsOperations.OvalAngleToPoint(rectX, rectY, rectWidth, rectHeight, -startAngle);
                    var endPoint = GraphicsOperations.OvalAngleToPoint(rectX, rectY, rectWidth, rectHeight, -endAngle);

                    if (figure == null)
                    {
                        figure = new PathFigure();
                        geometry.Figures.Add(figure);
                        figure.StartPoint = startPoint.ToPoint();
                    }
                    else
                    {
                        var lineSegment = new LineSegment()
                        {
                            Point = startPoint.ToPoint()
                        };
                        figure.Segments.Add(lineSegment);
                    }

                    var arcSegment = new ArcSegment()
                    {
                        Point = endPoint.ToPoint(),
                        Size = new UWPSize(rectWidth / 2, rectHeight / 2),
                        SweepDirection = clockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise,
                        IsLargeArc = absSweep >= 180,
                    };
                    figure.Segments.Add(arcSegment);
                }
                else if (operation == PathOperation.Close)
                {
                    figure.IsClosed = true;
                }
            }

            return geometry;
        }
    }
}
