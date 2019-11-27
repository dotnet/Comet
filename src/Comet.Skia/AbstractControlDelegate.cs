using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using SkiaSharp;

namespace Comet.Skia
{
	public abstract class AbstractControlDelegate : IControlDelegate
	{
		private DrawableControl _drawableControl;

		public event Action Invalidated;

		private readonly List<Action> _bindingInitializers = new List<Action>();
		private readonly List<string> _boundProperties = new List<string>();

		protected AbstractControlDelegate()
		{
			Mapper = new PropertyMapper<DrawableControl>();
		}

		protected AbstractControlDelegate(PropertyMapper<DrawableControl> mapper)
		{
			Mapper = mapper ?? new PropertyMapper<DrawableControl>();
		}

		public PropertyMapper<DrawableControl> Mapper { get; }

		public RectangleF Bounds { get; private set; }

		public void Invalidate()
		{
			Invalidated?.Invoke();
		}

		public abstract void Draw(SKCanvas canvas, RectangleF dirtyRect);

		public virtual void StartHoverInteraction(PointF[] points)
		{
		}

		public virtual void HoverInteraction(PointF[] points)
		{
		}

		public virtual void EndHoverInteraction()
		{
		}

		public virtual bool StartInteraction(PointF[] points)
		{
			return false;
		}

		public virtual void DragInteraction(PointF[] points)
		{

		}

		public virtual void EndInteraction(PointF[] points)
		{

		}

		public virtual void CancelInteraction()
		{

		}

		public virtual void Resized(RectangleF bounds)
		{
			Bounds = bounds;
		}

		public virtual void AddedToView(object nativeView, RectangleF bounds)
		{
			Bounds = bounds;
		}

		public void RemovedFromView(object view)
		{

		}

		public DrawableControl VirtualDrawableControl
		{
			get => _drawableControl;
			set
			{
				_drawableControl = value;
				if (_drawableControl != null)
				{
					foreach (var initializer in _bindingInitializers)
						initializer?.Invoke();

					_bindingInitializers.Clear();
				}
			}
		}

		public IDrawableControl NativeDrawableControl { get; set; }

		public virtual SizeF Measure(SizeF availableSize)
		{
			return availableSize;
		}

		public virtual void ViewPropertyChanged(string property, object value)
		{
			if (_boundProperties.Contains(property))
				Invalidate();
		}

		public static implicit operator View(AbstractControlDelegate controlDelegate)
		{
			return new DrawableControl(controlDelegate);
		}

		protected void SetBindingValue<T>(ref Binding<T> currentValue, Binding<T> newValue, [CallerMemberName] string propertyName = "")
		{
			currentValue = newValue;
			if (VirtualDrawableControl != null)
			{
				newValue?.BindToProperty((View)VirtualDrawableControl, propertyName);
				_boundProperties.Add(propertyName);
			}
			else
				_bindingInitializers.Add(
					() => {
						newValue?.BindToProperty((View)VirtualDrawableControl, propertyName);
						_boundProperties.Add(propertyName);
					});
		}
	}
}
