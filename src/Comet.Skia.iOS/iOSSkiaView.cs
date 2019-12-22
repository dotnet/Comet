using System;
using CoreGraphics;
using Foundation;
using SkiaSharp.Views.iOS;
using Comet.iOS;
using UIKit;

namespace Comet.Skia.iOS
{
    public class iOSSkiaView : SKCanvasView
    {
        private  SkiaView _virtualView;

        public iOSSkiaView()
        {
        }

        public iOSSkiaView(CGRect frame) : base(frame)
        {
            
        }

        public SkiaView VirtualView
        {
            get => _virtualView;
            set
            {
                if (_virtualView != null)
                    _virtualView.Invalidated -= HandleInvalidated;

                _virtualView = value;

                if (_virtualView != null)
                    _virtualView.Invalidated += HandleInvalidated;

                HandleInvalidated();
            }
        }

        private void HandleInvalidated()
        {
            if (Handle == IntPtr.Zero)
                return;
            
            SetNeedsDisplay();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            if (_virtualView == null) return;
            var canvas = e.Surface.Canvas;

            canvas.Save();
            var scale = CanvasSize.Width / (float)Bounds.Width;
            canvas.Scale(scale,scale);
            _virtualView.Draw(canvas, Bounds.ToRectangleF());
            canvas.Restore();
        }
        
        public override CGRect Frame
        {
            get => base.Frame;
            set
            {
                base.Frame = value;
                _virtualView?.Resized(Bounds.ToRectangleF());
            }
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            try
            {
                var viewPoints = this.GetPointsInView(evt);
                _virtualView?.StartInteraction(viewPoints);
            }
            catch (Exception exc)
            {
                Logger.Warn("An unexpected error occured handling a touch event within the control.", exc);
            }
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            try
            {
                var viewPoints = this.GetPointsInView(evt);
                _virtualView?.DragInteraction(viewPoints);
            }
            catch (Exception exc)
            {
                Logger.Warn("An unexpected error occured handling a touch moved event within the control.", exc);
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            try
            {
                var viewPoints = this.GetPointsInView(evt);
                _virtualView?.EndInteraction(viewPoints);
            }
            catch (Exception exc)
            {
                Logger.Warn("An unexpected error occured handling a touch ended event within the control.", exc);
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            try
            {
                _virtualView?.CancelInteraction();
            }
            catch (Exception exc)
            {
                Logger.Warn("An unexpected error occured cancelling the touches within the control.", exc);
            }
        }
    }
}
