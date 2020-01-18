using SkiaSharp.Views.UWP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Comet.Skia.UWP
{
	public class UWPSkiaView : SkiaSharp.Views.UWP.SKXamlCanvas
	{
		bool _inTouch;
		public UWPSkiaView()
		{
			this.SizeChanged += HandleSizeChanged;
			this.PointerEntered += HandlePointerEnter;
			this.PointerMoved += HandlePointerMoved;
			this.PointerExited += HandlePointerExited;
			this.PointerPressed += HandlePointerPressed;
			this.PointerReleased += HandlePointerReleased;
		}


		private PointF[] GetViewPoints(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			var point = e.GetCurrentPoint(this).RawPosition;
			return new[] { new PointF((float)point.X, (float)point.Y) };
		}

		private void HandlePointerEnter(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			if (!(VirtualView?.TouchEnabled ?? false))
				return;
			e.Handled = true;
			VirtualView?.StartHoverInteraction(GetViewPoints(e));
		}
		private void HandlePointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			if (!_inTouch) return;
			e.Handled = true;
			VirtualView?.DragInteraction(GetViewPoints(e));
		}

		private void HandlePointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			e.Handled = true;
			VirtualView?.EndHoverInteraction();
			VirtualView?.CancelInteraction();
			_inTouch = false;
		}

		private void HandlePointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			if (!(VirtualView?.TouchEnabled ?? false))
				return;
			e.Handled = true;
			_inTouch = VirtualView?.StartInteraction(GetViewPoints(e)) ?? false;
		}

		private void HandlePointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
		{
			if (!_inTouch) return;
			e.Handled = true;
			var points = GetViewPoints(e);
			var contained = VirtualView?.PointsContained(points) ?? false;
			VirtualView?.EndInteraction(points, contained);
			_inTouch = false;
		}

		private void HandleSizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
		{
			VirtualView?.Invalidate();
			VirtualView?.Resized(new System.Drawing.RectangleF(0, 0, (float)RenderSize.Width, (float)RenderSize.Height));
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
			this.Invalidate();
		}

		static long paintCount = 0;
		protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
		{
			base.OnPaintSurface(e);
			if (_virtualView == null) return;
			var canvas = e.Surface.Canvas;

			canvas.Save();
			var scale = CanvasSize.Width / (float)ActualSize.X;
			canvas.Scale(scale, scale);
			_virtualView.Draw(canvas, new System.Drawing.RectangleF(0, 0, ActualSize.X, ActualSize.Y));
			canvas.Restore();
			Debug.WriteLine($"Painting :{paintCount++}");
		}


	}
}
