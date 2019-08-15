using System;
using SkiaSharp.Views.Android;
using Android.Content;
using Android.Views;

namespace Comet.Skia.Android
{
    public class AndroidDrawableControl : SKCanvasView, IDrawableControl
    {
        private  IControlDelegate _controlDelegate;
        private RectangleF _bounds;
        private bool _dragStarted;
        private PointF[] _lastMovedViewPoints;

        public AndroidDrawableControl(Context context) : base(context)
        {
            SetBackgroundColor(global::Android.Graphics.Color.Transparent);

            if ((int)global::Android.OS.Build.VERSION.SdkInt < 21)
                SetLayerType(global::Android.Views.LayerType.Software, null);
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
                    _bounds = new RectangleF(0, 0, MeasuredWidth, MeasuredHeight);
                    _controlDelegate.AddedToView(this, _bounds);
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
            
            Invalidate();
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);

            if (changed)
            {
                var width = right - left;
                var height = bottom - top;
                _bounds = new RectangleF(left, top, width, height);
                _controlDelegate?.Resized(_bounds);
            }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            if (_controlDelegate == null) return;
            var canvas = e.Surface.Canvas;

            canvas.Save();
            var scale = CanvasSize.Width / (float)_bounds.Width;
            canvas.Scale(scale,scale);
            _controlDelegate.Draw(canvas, _bounds);
            canvas.Restore();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            try
            {
                if (e == null) throw new ArgumentNullException(nameof(e));

                int touchCount = e.PointerCount;
                var touchPoints = new PointF[touchCount];

                for (int i = 0; i < touchCount; i++)
                    touchPoints[i] = new PointF(e.GetX(i), e.GetY(i));

                var actionMasked = e.Action & MotionEventActions.Mask;

                switch (actionMasked)
                {
                    case MotionEventActions.Move:
                        TouchesMoved(touchPoints, e);
                        break;
                    case MotionEventActions.Down:
                    case MotionEventActions.PointerDown:
                        TouchesBegan(touchPoints, e);
                        break;
                    case MotionEventActions.Up:
                    case MotionEventActions.PointerUp:
                        TouchesEnded(touchPoints, e);
                        break;
                    case MotionEventActions.Cancel:
                        TouchesCanceled();
                        break;
                }
            }
            catch (Exception exc)
            {
                Logger.Error("An unexpected error occured handling a touch event within the control.", exc);
            }

            return true;
        }

        public void TouchesBegan(PointF[] points, MotionEvent e)
        {
            _dragStarted = false;
            _lastMovedViewPoints = points;
            ControlDelegate?.StartInteraction(points);
        }

        public void TouchesMoved(PointF[] points, MotionEvent e)
        {
            if (!_dragStarted)
            {
                if (points.Length == 1)
                {
                    float deltaX = _lastMovedViewPoints[0].X - points[0].X;
                    float deltaY = _lastMovedViewPoints[0].Y - points[0].Y;

                    if (Math.Abs(deltaX) <= 3 && Math.Abs(deltaY) <= 3)
                        return;
                }
            }

            _lastMovedViewPoints = points;
            _dragStarted = true;
            ControlDelegate?.DragInteraction(points);
        }

        public void TouchesEnded(PointF[] points, MotionEvent e)
        {
            ControlDelegate?.EndInteraction(points);
        }

        public void TouchesCanceled()
        {
            ControlDelegate?.CancelInteraction();
        }
    }
}
