using System;
using SkiaSharp;

namespace Comet.Skia
{
    public interface IControlDelegate
    {
        event Action Invalidated;
        PropertyMapper<DrawableControl> Mapper { get; }
        RectangleF Bounds { get; }
        DrawableControl VirtualDrawableControl { get; set; }
        IDrawableControl NativeDrawableControl { get; set; }
        void Invalidate();
        void Draw(SKCanvas canvas, RectangleF dirtyRect);
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
        SizeF Measure(SizeF availableSize);
        void ViewPropertyChanged(string property, object value);
    }
}