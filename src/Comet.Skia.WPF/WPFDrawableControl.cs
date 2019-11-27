using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using Size = System.Windows.Size;

namespace Comet.Skia.WPF
{
	public class WPFDrawableControl : SKElement, IDrawableControl
    {
        private IControlDelegate _controlDelegate;
        private RectangleF _bounds;
        private bool _inTouch;

        public WPFDrawableControl()
        {
            PreviewMouseDown += OnPreviewMouseDown;
            PreviewMouseUp += OnPreviewMouseUp;
            MouseMove += OnMouseMove;
            MouseLeave += OnMouseLeave;

            SizeChanged += HandleSizeChanged;
        }

        public IControlDelegate ControlDelegate
        {
            get => _controlDelegate;
            set
            {
                if (_controlDelegate != null)
                {
                    _controlDelegate.Invalidated -= HandleDelegateInvalidated;
                    _controlDelegate.RemovedFromView(this);
                    _controlDelegate.NativeDrawableControl = null;
                }

                _controlDelegate = value;

                if (_controlDelegate != null)
                {
                    _bounds = new RectangleF(0, 0, (float)RenderSize.Width, (float)RenderSize.Height);
                    _controlDelegate.AddedToView(this, _bounds);
                    _controlDelegate.Invalidated += HandleDelegateInvalidated;
                    _controlDelegate.NativeDrawableControl = this;
                }

                Invalidate();
            }
        }

        private void HandleSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            var size = e.NewSize;
            if (size.Width <= 0 || size.Height <= 0 || double.IsNaN(size.Width) || double.IsNaN(size.Height)) return;
            _bounds = new RectangleF(0, 0, (float)size.Width, (float)size.Height);
            _controlDelegate?.Resized(_bounds);
            Invalidate();
        }

        private void HandleDelegateInvalidated() => Invalidate();

        public void Invalidate()
        {
            var renderSize = RenderSize;
            if (renderSize.Width <= 0 || renderSize.Height <= 0 || double.IsNaN(renderSize.Width) || double.IsNaN(renderSize.Height)) return;

            InvalidateVisual();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            if (_controlDelegate == null) return;
            var canvas = e.Surface.Canvas;

            canvas.Save();
            var scale = CanvasSize.Width / (float)_bounds.Width;
            canvas.Scale(scale, scale);
            _controlDelegate.Draw(canvas, _bounds);
            canvas.Restore();
        }

        private PointF[] GetViewPoints(MouseButtonEventArgs evt)
        {
            var point = evt.GetPosition(this);
            return new PointF[] { new PointF((float)point.X, (float)point.Y) };
        }

        private PointF[] GetViewPoints(MouseEventArgs evt)
        {
            var point = evt.GetPosition(this);
            return new PointF[] { new PointF((float)point.X, (float)point.Y) };
        }

        protected virtual void OnPreviewMouseUp(object sender, MouseButtonEventArgs evt)
        {
            if (!_inTouch) return;
            evt.Handled = true;
            _controlDelegate?.EndInteraction(GetViewPoints(evt));
            _inTouch = false;
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs evt)
        {
            evt.Handled = true;
            _inTouch = _controlDelegate?.StartInteraction(GetViewPoints(evt)) ?? false;
        }

        private void OnMouseMove(object sender, MouseEventArgs evt)
        {
            if (!_inTouch) return;
            evt.Handled = true;
            _controlDelegate?.DragInteraction(GetViewPoints(evt));
        }

        private void OnMouseLeave(object sender, MouseEventArgs evt)
        {
            if (!_inTouch) return;
            evt.Handled = true;
            _controlDelegate?.CancelInteraction();
            _inTouch = false;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var size = base.ArrangeOverride(arrangeSize);
            Invalidate();
            return size;
        }
    }
}
