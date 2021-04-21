using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public interface IGraphicsControl : IViewHandler, IDrawable
	{
		void Invalidate();
		string[] LayerDrawingOrder();
		DrawMapper DrawMapper { get; }
		void StartHoverInteraction(PointF[] points);
		void HoverInteraction(PointF[] points);
		void EndHoverInteraction();
		bool StartInteraction(PointF[] points);
		void DragInteraction(PointF[] points);
		void EndInteraction(PointF[] points, bool inside);
		void CancelInteraction();
		void Resized(RectangleF bounds);
	}
}
