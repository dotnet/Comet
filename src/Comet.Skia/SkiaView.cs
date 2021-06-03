using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using SkiaSharp;

namespace Comet.Skia
{
	public class SkiaView : View
	{
		public event Action Invalidated;

		private readonly List<string> _boundProperties = new List<string>();

		public SkiaView() : this(new PropertyMapper<SkiaView>())
		{

		}

		public SkiaView(PropertyMapper<SkiaView> mapper)
		{
			Mapper = mapper ?? new PropertyMapper<SkiaView>();
		}

		internal BindingState GetState() => State;

		public PropertyMapper<SkiaView> Mapper { get; }

		public RectangleF Bounds { get; private set; }

		public void Invalidate()
		{
			if(Invalidated != null)
				ThreadHelper.RunOnMainThread(Invalidated);
		}
		public bool PointsContained(PointF[] points) => points.Any(p => Bounds.BoundsContains(p));

		public bool TouchEnabled { get; set; } = true;

		public Action<SKCanvas, RectangleF> OnDraw;

		public virtual void Draw(SKCanvas canvas, RectangleF dirtyRect) => OnDraw?.Invoke(canvas, dirtyRect);

		protected PointF CurrentTouchPoint { get; set; }
		ControlState currentState = ControlState.Default;
		public ControlState CurrentState
		{
			get => currentState;
			set
			{
				if (currentState == value)
					return;
				currentState = value;
				ControlStateChanged();
			}
		}
		public virtual void StartHoverInteraction(PointF[] points)
		{
			CurrentTouchPoint = points.FirstOrDefault();
			CurrentState = ControlState.Hovered;
		}

		public virtual void HoverInteraction(PointF[] points)
		{
		}

		public virtual void EndHoverInteraction()
		{
		}

		public virtual bool StartInteraction(PointF[] points)
		{
			CurrentTouchPoint = points.FirstOrDefault();
			CurrentState = ControlState.Pressed;
			return true;
		}

		public virtual void DragInteraction(PointF[] points)
		{

			CurrentTouchPoint = points.FirstOrDefault();
		}

		public virtual void EndInteraction(PointF[] points, bool inside)
		{
			CurrentState = ControlState.Default;
		}

		public virtual void CancelInteraction()
		{
			CurrentState = ControlState.Default;

		}

		protected virtual void ControlStateChanged()
		{
			
		}

		public virtual void Resized(RectangleF bounds)
		{
			Bounds = bounds;
		}

		public override void ViewPropertyChanged(string property, object value)
		{
			base.ViewPropertyChanged(property, value);
			//if (_boundProperties.Contains(property))
			Invalidate();
		}

		protected void SetBindingValue<T>(
			ref Binding<T> currentValue,
			Binding<T> newValue,
			[CallerMemberName] string propertyName = "")
		{
			currentValue = newValue;
			newValue?.BindToProperty(this, propertyName);
			_boundProperties.Add(propertyName);
		}
	}
}