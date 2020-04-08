using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using System.Maui.UWP;
using SkiaSharp.Views.UWP;
using System.Drawing;

namespace System.Maui.Skia.UWP
{
	public class UWPDrawableControl : global::SkiaSharp.Views.UWP.SKXamlCanvas, IDrawableControl
	{
		private readonly List<uint> _touchIds = new List<uint>();
		private readonly List<PointF> _touchPoints = new List<PointF>();

		private IControlDelegate _controlDelegate;
		private RectangleF _bounds;
		private bool _inTouch;

		public UWPDrawableControl()
		{
			PointerPressed += OnPointerPressed;
			PointerCanceled += OnPointerCanceled;
			PointerMoved += OnPointerMoved;
			PointerReleased += OnPointerReleased;
			PointerEntered += OnPointerEntered;
			PointerExited += OnPointerExited;

			SizeChanged += HandleSizeChanged;
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
					_bounds = new RectangleF(0, 0, (float)RenderSize.Width, (float)RenderSize.Height);
					_controlDelegate.AddedToView(this, _bounds);
					_controlDelegate.Invalidated += HandleInvalidated;
					_controlDelegate.NativeDrawableControl = this;
				}

				HandleInvalidated();
			}
		}

		private void HandleInvalidated() => Invalidate();

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

		private void HandleSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var size = e.NewSize;
			if (size.Width <= 0 || size.Height <= 0 || double.IsNaN(size.Width) || double.IsNaN(size.Height)) return;
			_bounds = new RectangleF(0, 0, (float)size.Width, (float)size.Height);
			_controlDelegate?.Resized(_bounds);
			Invalidate();
		}

		private PointF[] GetPointsInView(
			  PointerRoutedEventArgs evt)
		{
			evt.Handled = true;
			var point = evt.GetCurrentPoint(this).ToPointF();

			var index = _touchIds.IndexOf(evt.Pointer.PointerId);
			if (index >= 0)
			{
				_touchPoints[index] = point;
			}
			else
			{
				_touchIds.Add(evt.Pointer.PointerId);
				_touchPoints.Add(point);
			}

			return _touchPoints.ToArray();
		}

		private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
		{
			var viewPoints = GetPointsInView(e);
			_controlDelegate?.StartHoverInteraction(viewPoints);
		}

		private void OnPointerExited(object sender, PointerRoutedEventArgs e) => _controlDelegate?.EndHoverInteraction();

		protected void OnPointerPressed(
			object source,
			PointerRoutedEventArgs evt)
		{
			// Ignore right clicks
			if (evt.GetCurrentPoint(this).Properties.IsRightButtonPressed)
				return;

			try
			{
				var viewPoints = GetPointsInView(evt);
				_inTouch = _controlDelegate?.StartInteraction(viewPoints) ?? false;
				if (_inTouch)
				{
					evt.Handled = true;
					((UIElement)source).CapturePointer(evt.Pointer);
				}
			}
			catch (Exception exc)
			{
				Logger.Warn("An unexpected error occured handling a touch event within the control.", exc);
			}
		}

		protected void OnPointerMoved(
			object source,
			PointerRoutedEventArgs evt)
		{
			try
			{
				var viewPoints = GetPointsInView(evt);

				if (_inTouch)
				{
					_controlDelegate?.DragInteraction(viewPoints);
				}
				else
				{
					_controlDelegate?.HoverInteraction(viewPoints);
				}
			}
			catch (Exception exc)
			{
				Logger.Warn("An unexpected error occured handling a touch event within the control.", exc);
			}
		}

		protected void OnPointerReleased(
			object source,
			PointerRoutedEventArgs evt)
		{
			if (!_inTouch)
				return;

			// Ignore right clicks
			if (evt.GetCurrentPoint(this).Properties.IsRightButtonPressed)
				return;

			try
			{
				var viewPoints = GetPointsInView(evt);
				_controlDelegate?.EndInteraction(viewPoints);
			}
			catch (Exception exc)
			{
				Logger.Warn("An unexpected error occured handling a touch event within the control.", exc);
			}

			var index = _touchIds.IndexOf(evt.Pointer.PointerId);
			if (index >= 0)
			{
				_touchIds.RemoveAt(index);
				_touchPoints.RemoveAt(index);
			}

			_inTouch = _touchPoints.Count > 0;
		}

		protected void OnPointerCanceled(
			object source,
			PointerRoutedEventArgs e)
		{
			_touchIds.Clear();
			_touchPoints.Clear();
			_inTouch = false;

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
