using System;
using SkiaSharp;

namespace HotUI.Skia
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
        bool StartInteraction(PointF[] points);
        void DragInteraction(PointF[] points);
        void EndInteraction(PointF[] points);
        void CancelInteraction();
        void Resized(RectangleF bounds);
        void AddedToView(object view, RectangleF bounds);
        void RemovedFromView(object view);
        SizeF Measure(SizeF availableSize);
    }
}