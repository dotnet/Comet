using System.Collections.Generic;
using System.Drawing;
using System.Maui.Skia;
using SkiaSharp;

namespace System.Maui.Samples.Skia
{
	public class SimpleFingerPaint : AbstractControlDelegate
	{
		private readonly List<List<PointF>> _pointsLists = new List<List<PointF>>();

		public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
		{
			canvas.Clear(SKColors.White);

			var paint = new SKPaint()
			{
				Color = SKColors.Blue,
				StrokeWidth = 2
			};

			foreach (var pointsList in _pointsLists)
			{
				for (var i = 0; i < pointsList.Count; i++)
				{
					var point = pointsList[i];
					if (i > 0)
					{
						var lastPoint = pointsList[i - 1];
						canvas.DrawLine(lastPoint.X, lastPoint.Y, point.X, point.Y, paint);
					}
				}
			}
		}

		public override bool StartInteraction(PointF[] points)
		{
			var pointsList = new List<PointF> { points[0] };
			_pointsLists.Add(pointsList);

			Invalidate();
			return true;
		}

		public override void DragInteraction(PointF[] points)
		{
			var pointsList = _pointsLists[_pointsLists.Count - 1];
			pointsList.Add(points[0]);

			Invalidate();
		}

		public override void EndInteraction(PointF[] points)
		{
			var pointsList = _pointsLists[_pointsLists.Count - 1];
			pointsList.Add(points[0]);

			Invalidate();
		}

		public void Reset()
		{
			_pointsLists.Clear();
			Invalidate();
		}
	}
}
