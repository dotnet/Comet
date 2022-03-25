using System.Collections.Generic;

namespace Comet.Samples
{
	public class SimpleFingerPaint : GraphicsView, IDrawable, IGraphicsView
	{
		protected readonly List<List<PointF>> _pointsLists = new List<List<PointF>>();

		void IDrawable.Draw(ICanvas canvas, RectF dirtyRect)
		{
			//var paint = new SolidPaint(Colors.Blue);
			canvas.StrokeColor = Colors.Blue;
			canvas.StrokeSize = 2;

			foreach (var pointsList in _pointsLists)
			{
				for (var i = 0; i < pointsList.Count; i++)
				{
					var point = pointsList[i];
					if (i > 0)
					{
						var lastPoint = pointsList[i - 1];
						canvas.DrawLine(lastPoint.X, lastPoint.Y, point.X, point.Y);
					}
				}
			}
		}

		void IGraphicsView.StartInteraction(PointF[] points)
		{
			var pointsList = new List<PointF> { points[0] };
			_pointsLists.Add(pointsList);
			//ViewHandler.
		}

		void IGraphicsView.DragInteraction(PointF[] points)
		{
			var pointsList = _pointsLists[_pointsLists.Count - 1];
			pointsList.Add(points[0]);

			Invalidate();
		}

		void IGraphicsView.EndInteraction(PointF[] points, bool contained)
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
