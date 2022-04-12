using System;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class GraphicsView : View, IDrawable, IGraphicsView
	{
		public Action<ICanvas, RectF> Draw { get; set; }
		public Action CancelInteraction { get; set; }
		public Action<PointF[]> DragInteraction { get; set; }
		public Action EndHoverInteraction { get; set; }
		public Action<PointF[],bool> EndInteraction { get; set; }
		public Action<PointF[]> MoveHoverInteraction { get; set; }
		public Action<PointF[]> StartHoverInteraction { get; set; }
		public Action<PointF[]> StartInteraction { get; set; }

		public void Invalidate() => ViewHandler?.Invoke(nameof(IGraphicsView.Invalidate));

		IDrawable IGraphicsView.Drawable => this;

		void IDrawable.Draw(ICanvas canvas, RectF dirtyRect) =>  Draw?.Invoke(canvas, dirtyRect);

		void IGraphicsView.CancelInteraction() => CancelInteraction?.Invoke();
		void IGraphicsView.DragInteraction(PointF[] points) => DragInteraction?.Invoke(points);
		void IGraphicsView.EndHoverInteraction() => EndHoverInteraction?.Invoke();
		void IGraphicsView.EndInteraction(PointF[] points, bool isInsideBounds) => EndInteraction?.Invoke(points, isInsideBounds);
		void IGraphicsView.Invalidate() { }
		void IGraphicsView.MoveHoverInteraction(PointF[] points) => MoveHoverInteraction?.Invoke(points);
		void IGraphicsView.StartHoverInteraction(PointF[] points) => StartHoverInteraction?.Invoke(points);
		void IGraphicsView.StartInteraction(PointF[] points) => StartInteraction?.Invoke(points);
	}
}
