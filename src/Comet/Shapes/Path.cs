using System;
using System.Collections.Generic;
using System.Linq;
using Comet.Graphics;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class Path : Shape
	{
		private readonly PathF _path;
		private readonly PathScaling _scaling;

		public Path(PathF path, PathScaling scaling = PathScaling.AspectFit)
		{
			_path = path;
			_scaling = scaling;
		}

		public Path(string path, PathScaling scaling = PathScaling.AspectFit)
		{
			_path = PathBuilder.Build(path);
			_scaling = scaling;
		}
		RectangleF? cachedPathBounds;
		RectangleF PathBounds => cachedPathBounds ??= CalculateBounds();
		RectangleF CalculateBounds()
		{
			var xValues = new List<float>();
			var yValues = new List<float>();
			var _points = _path.Points.ToList();

			int pointIndex = 0;
			int arcAngleIndex = 0;
			int arcClockwiseIndex = 0;

			foreach (var operation in _path.SegmentTypes)
			{
				if (operation == PathOperation.Move)
				{
					pointIndex++;
				}
				else if (operation == PathOperation.Line)
				{
					var startPoint = _points[pointIndex - 1];
					var endPoint = _points[pointIndex++];

					xValues.Add(startPoint.X);
					xValues.Add(endPoint.X);
					yValues.Add(startPoint.Y);
					yValues.Add(endPoint.Y);
				}
				else if (operation == PathOperation.Quad)
				{
					var startPoint = _points[pointIndex - 1];
					var controlPoint = _points[pointIndex++];
					var endPoint = _points[pointIndex++];

					var bounds = GraphicsOperations.GetBoundsOfQuadraticCurve(startPoint, controlPoint, endPoint);

					xValues.Add(bounds.Left);
					xValues.Add(bounds.Right);
					yValues.Add(bounds.Top);
					yValues.Add(bounds.Bottom);
				}
				else if (operation == PathOperation.Cubic)
				{
					var startPoint = _points[pointIndex - 1];
					var controlPoint1 = _points[pointIndex++];
					var controlPoint2 = _points[pointIndex++];
					var endPoint = _points[pointIndex++];

					var bounds = GraphicsOperations.GetBoundsOfCubicCurve(startPoint, controlPoint1, controlPoint2, endPoint);

					xValues.Add(bounds.Left);
					xValues.Add(bounds.Right);
					yValues.Add(bounds.Top);
					yValues.Add(bounds.Bottom);
				}
				else if (operation == PathOperation.Arc)
				{
					var topLeft = _points[pointIndex++];
					var bottomRight = _points[pointIndex++];
					float startAngle = _path.GetArcAngle(arcAngleIndex++);
					float endAngle = _path.GetArcAngle(arcAngleIndex++);
					var clockwise = _path.GetArcClockwise(arcClockwiseIndex++);

					var bounds = GraphicsOperations.GetBoundsOfArc(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y, startAngle, endAngle, clockwise);

					xValues.Add(bounds.Left);
					xValues.Add(bounds.Right);
					yValues.Add(bounds.Top);
					yValues.Add(bounds.Bottom);
				}
			}

			var minX = xValues.Min();
			var minY = yValues.Min();
			var maxX = xValues.Max();
			var maxY = yValues.Max();

			return new RectangleF(minX, minY, maxX - minX, maxY - minY);
		}
		public override PathF PathForBounds(Rectangle rectd, float density = 1)
		{

			var bounds = PathBounds;
			RectangleF rect = rectd;

			AffineTransform transform = null;

			if (_scaling == PathScaling.AspectFit)
			{
				var factorX = rect.Width / bounds.Width;
				var factorY = rect.Height / bounds.Height;
				var factor = Math.Min(factorX, factorY);

				var width = bounds.Width * factor;
				var height = bounds.Height * factor;
				var translateX = (rect.Width - width) / 2 + rect.X;
				var translateY = (rect.Height - height) / 2 + rect.Y;

				transform = AffineTransform.GetTranslateInstance(-bounds.X, -bounds.Y);
				transform.Translate(translateX, translateY);
				transform.Scale(factor, factor);
			}
			else if (_scaling == PathScaling.AspectFill)
			{
				var factorX = rect.Width / bounds.Width;
				var factorY = rect.Height / bounds.Height;
				var factor = Math.Max(factorX, factorY);

				var width = bounds.Width * factor;
				var height = bounds.Height * factor;
				var translateX = (rect.Width - width) / 2 + rect.X;
				var translateY = (rect.Height - height) / 2 + rect.Y;

				transform = AffineTransform.GetTranslateInstance(-bounds.X, -bounds.Y);
				transform.Translate(translateX, translateY);
				transform.Scale(factor, factor);
			}
			else if (_scaling == PathScaling.Fill)
			{
				var factorX = rect.Width / bounds.Width;
				var factorY = rect.Height / bounds.Height;
				transform = AffineTransform.GetScaleInstance(factorX, factorY);

				var translateX = bounds.X * factorX + rect.X;
				var translateY = bounds.Y * factorY + rect.Y;
				transform.Translate(translateX, translateY);
			}
			else
			{
				var width = bounds.Width;
				var height = bounds.Height;
				var translateX = (rect.Width - width) / 2 + rect.X;
				var translateY = (rect.Height - height) / 2 + rect.Y;

				transform = AffineTransform.GetTranslateInstance(-bounds.X, -bounds.Y);
				transform.Translate(translateX, translateY);
			}

			if (!transform?.IsIdentity ?? false)
				_path.Transform(transform);

			return _path;
		}
	}
}
