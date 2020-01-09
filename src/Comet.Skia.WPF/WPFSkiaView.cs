using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using Size = System.Windows.Size;
using System.Linq;
using System.Diagnostics;

namespace Comet.Skia.WPF
{
	public class WPFSkiaView : SkiaSharp.Views.WPF.SKElement
	{
		private bool _inTouch;

		public WPFSkiaView()
        {

			PreviewMouseDown += OnPreviewMouseDown;
			PreviewMouseUp += OnPreviewMouseUp;
			MouseMove += OnMouseMove;
			MouseLeave += OnMouseLeave;
            MouseEnter += OnMouseEnter;

			SizeChanged += HandleSizeChanged;
		}

        SkiaView _virtualView;
		public SkiaView VirtualView
		{
			get => _virtualView;
			set
			{
				if (_virtualView != null)
				{
					_virtualView.Invalidated -= HandleInvalidated;
					_virtualView.NeedsLayout -= NeedsLayout;
				}

				_virtualView = value;

				if (_virtualView != null)
				{
					_virtualView.Invalidated += HandleInvalidated;
					_virtualView.NeedsLayout += NeedsLayout;
				}

				HandleInvalidated();
			}
		}
		private void NeedsLayout(object sender, EventArgs e)
		{

		}

		private void HandleInvalidated()
		{
			var control = _virtualView as SkiaControl;
			if (control != null)
			{
				//this.AccessibilityLabel = control.AccessibilityText();
				////this.AccessibilityTraits = UIAccessibilityTrait.StaticText;
				//this.IsAccessibilityElement = true;
			}
			this.InvalidateVisual();
		}

		static long paintCount = 0;
		protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
		{
			base.OnPaintSurface(e);
			if (_virtualView == null) return;
			var canvas = e.Surface.Canvas;
			canvas.Save();
			var scale = CanvasSize.Width / (float)this.RenderSize.Width;
			canvas.Scale(scale, scale);
			_virtualView.Draw(canvas, new System.Drawing.RectangleF(0, 0, (float)RenderSize.Width, (float)RenderSize.Height));
			canvas.Restore();
			Debug.WriteLine($"Painting :{paintCount++}");
		}

		private void HandleSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{
			VirtualView?.Invalidate();
			VirtualView?.Resized(new System.Drawing.RectangleF(0, 0, (float)RenderSize.Width, (float)RenderSize.Height));
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
			var points = GetViewPoints(evt);
			var contained = VirtualView?.PointsContained(points) ?? false;
			VirtualView?.EndInteraction(points, contained);
			_inTouch = false;
		}

		private void OnPreviewMouseDown(object sender, MouseButtonEventArgs evt)
		{
			if (!(VirtualView?.TouchEnabled ?? false))
				return;
			evt.Handled = true;
			_inTouch = VirtualView?.StartInteraction(GetViewPoints(evt)) ?? false;
		}

		private void OnMouseMove(object sender, MouseEventArgs evt)
		{
			if (!_inTouch) return;
			evt.Handled = true;
			VirtualView?.DragInteraction(GetViewPoints(evt));
		}

		private void OnMouseLeave(object sender, MouseEventArgs evt)
		{
			//if (!_inTouch) return;
			evt.Handled = true;
			VirtualView?.EndHoverInteraction();
			VirtualView?.CancelInteraction();
			_inTouch = false;
		}

		private void OnMouseEnter(object sender, MouseEventArgs evt)
		{
			if (!(VirtualView?.TouchEnabled ?? false))
				return;
			evt.Handled = true;
			VirtualView?.StartHoverInteraction(GetViewPoints(evt));

		}

	}
}
