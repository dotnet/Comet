using System;
using CoreGraphics;
using SkiaSharp.Views.Mac;
using HotUI.Mac;
using AppKit;

namespace HotUI.Skia.Mac
{
    public class MacDrawableControl : SKCanvasView, IDrawableControl
    {
        private  IControlDelegate _controlDelegate;
        
        public MacDrawableControl()
        {
        }

        public MacDrawableControl(CGRect frame) : base(frame)
        {
        }

        public override bool IsFlipped => true;

        public  IControlDelegate ControlDelegate
        {
            get => _controlDelegate;
            set
            {
                if (_controlDelegate != null)
                {
                    _controlDelegate.Invalidated -= HandleInvalidated;
                    _controlDelegate.RemovedFromView(this);
                    _controlDelegate.NativeDrawableControl = null;
                }

                _controlDelegate = value;

                if (_controlDelegate != null)
                {
                    _controlDelegate.AddedToView(this, Bounds.ToRectangleF());
                    _controlDelegate.Invalidated += HandleInvalidated;
                    _controlDelegate.NativeDrawableControl = this;
                }

                HandleInvalidated();
            }
        }

        private void HandleInvalidated()
        {
            if (Handle == IntPtr.Zero)
                return;

            NeedsDisplay = true;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            if (_controlDelegate == null) return;
            var canvas = e.Surface.Canvas;

            canvas.Save();
            var scale = CanvasSize.Width / (float)Bounds.Width;
            canvas.Scale(scale,-scale);
            canvas.Translate(0, -(float)Bounds.Height);
            _controlDelegate.Draw(canvas, Bounds.ToRectangleF());
            canvas.Restore();
        }
        
        public override CGRect Frame
        {
            get => base.Frame;
            set
            {
                base.Frame = value;
                _controlDelegate?.Resized(Bounds.ToRectangleF());
            }
        }

        public override void MouseDown(NSEvent theEvent)
        {
            try
            {
                var windowPoint = theEvent.LocationInWindow;
                var pointInView = ConvertPointFromView(windowPoint, Window.ContentView);
                var points = new PointF[] { pointInView.ToPointF() };
                _controlDelegate?.StartInteraction(points);
            }
            catch (Exception exc)
            {
                Logger.Error("An unexpected error occured handling a mouse event within the control.", exc);
            }
        }

        public override void MouseDragged(NSEvent theEvent)
        {
            try
            {
                var windowPoint = theEvent.LocationInWindow;
                var pointInView = ConvertPointFromView(windowPoint, Window.ContentView);
                var points = new PointF[] { pointInView.ToPointF() };
                _controlDelegate?.DragInteraction(points);
            }
            catch (Exception exc)
            {
                Logger.Error("An unexpected error occured handling a mouse moved event within the control.", exc);
            }
        }

        public override void MouseUp(NSEvent theEvent)
        {
            try
            {
                var windowPoint = theEvent.LocationInWindow;
                var pointInView = ConvertPointFromView(windowPoint, Window.ContentView);
                var points = new PointF[] { pointInView.ToPointF() };
                _controlDelegate?.EndInteraction(points);
            }
            catch (Exception exc)
            {
                Logger.Error("An unexpected error occured handling a mouse up event within the control.", exc);
            }
        }
    }
}
