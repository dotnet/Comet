using System;
using Comet.GraphicsControls;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.GraphicsControls
{
	public interface IGraphicsControl : IViewHandler, IDrawable
	{
		void Invalidate();
		string[] LayerDrawingOrder();
		DrawMapper DrawMapper { get; }
		bool TouchEnabled { get; }
		void StartHoverInteraction(PointF[] points);
		void HoverInteraction(PointF[] points);
		void EndHoverInteraction();
		bool StartInteraction(PointF[] points);
		void DragInteraction(PointF[] points);
		void EndInteraction(PointF[] points, bool inside);
		void CancelInteraction();
		bool PointsContained(PointF[] points);
		void Resized(RectangleF bounds);
	}
}
