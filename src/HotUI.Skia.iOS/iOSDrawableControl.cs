using System;
using CoreGraphics;
using Foundation;
using SkiaSharp.Views.iOS;
using HotUI.iOS;
using UIKit;

namespace HotUI.Skia.iOS
{
    public class iOSDrawableControl : SKCanvasView, IDrawableControl
    {
        private  IControlDelegate _controlDelegate;
        
        public iOSDrawableControl()
        {
        }

        public iOSDrawableControl(CGRect frame) : base(frame)
        {
            
        }

        public IControlDelegate ControlDelegate
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
            
            SetNeedsDisplay();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            if (_controlDelegate == null) return;
            var canvas = e.Surface.Canvas;

            canvas.Save();
            var scale = CanvasSize.Width / (float)Bounds.Width;
            canvas.Scale(scale,scale);
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

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            try
            {
                var viewPoints = this.GetPointsInView(evt);
                _controlDelegate?.StartInteraction(viewPoints);
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
                _controlDelegate?.DragInteraction(viewPoints);
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
                _controlDelegate?.EndInteraction(viewPoints);
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
                _controlDelegate?.CancelInteraction();
            }
            catch (Exception exc)
            {
                Logger.Warn("An unexpected error occured cancelling the touches within the control.", exc);
            }
        }
    }
}
