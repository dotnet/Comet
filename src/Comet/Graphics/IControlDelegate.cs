using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.Graphics
{
	public interface IControlDelegate
	{
		event Action Invalidated;
		PropertyMapper<DrawableControl> Mapper { get; }
		RectangleF Bounds { get; }
		DrawableControl VirtualDrawableControl { get; set; }
		IDrawableControl NativeDrawableControl { get; set; }
		void Invalidate();
		void Draw(ICanvas canvas, RectangleF dirtyRect);
		void StartHoverInteraction(PointF[] points);
		void HoverInteraction(PointF[] points);
		void EndHoverInteraction();
		bool StartInteraction(PointF[] points);
		void DragInteraction(PointF[] points);
		void EndInteraction(PointF[] points);
		void CancelInteraction();
		void Resized(RectangleF bounds);
		void AddedToView(object nativeView, RectangleF bounds);
		void RemovedFromView(object nativeView);
		SizeF GetIntrinsicSize(SizeF availableSize);
		void ViewPropertyChanged(string property, object value);
	}
}
