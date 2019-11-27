using System;
using CoreGraphics;
using SkiaSharp.Views.Mac;
using Comet.Mac;
using AppKit;
using System.Linq;
using System.Drawing;

namespace Comet.Skia.Mac
{
	public class MacSkiaView : SKCanvasView
	{
		private SkiaView _virtualView;

		public MacSkiaView()
		{
		}

		public MacSkiaView(CGRect frame) : base(frame)
		{
		}

		public override bool IsFlipped => true;

		public SkiaView VirtualView
		{
			get => _virtualView;
			set
			{
				if (_virtualView != null)
				{
					_virtualView.Invalidated -= HandleInvalidated;
				}

				_virtualView = value;

				if (_virtualView != null)
				{
					_virtualView.Invalidated += HandleInvalidated;
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
			if (_virtualView == null) return;
			var canvas = e.Surface.Canvas;

			canvas.Save();
			var scale = CanvasSize.Width / (float)Bounds.Width;
			canvas.Scale(scale, -scale);
			canvas.Translate(0, -(float)Bounds.Height);
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
		bool PointsContained(PointF[] points) => points.Any(p => VirtualView.Frame.BoundsContains(p));
		bool pressedContained = false;
		public override void MouseDown(NSEvent theEvent)
		{
			try
			{
				pressedContained = true;
				var windowPoint = theEvent.LocationInWindow;
				var pointInView = ConvertPointFromView(windowPoint, Window.ContentView);
				var points = new PointF[] { pointInView.ToPointF() };
				_virtualView?.StartInteraction(points);
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
				pressedContained = PointsContained(points);
				_virtualView?.DragInteraction(points);
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
				_virtualView?.EndInteraction(points, pressedContained);
			}
			catch (Exception exc)
			{
				Logger.Error("An unexpected error occured handling a mouse up event within the control.", exc);
			}
		}
	}
}
