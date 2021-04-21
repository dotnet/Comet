using System;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Comet.GraphicsControls
{
	public abstract partial class GraphicsControlHandler<TViewDrawable, TVirtualView> : IGraphicsControl
		where TVirtualView : class, IView
		where TViewDrawable : class, IViewDrawable
	{
		protected readonly DrawMapper drawMapper;


		protected GraphicsControlHandler() : base(GraphicsControls.ViewHandler.Mapper)
		{
			drawMapper = GraphicsControls.ViewHandler.DrawMapper;
		}

		protected GraphicsControlHandler(DrawMapper drawMapper, PropertyMapper mapper) : base(mapper ?? GraphicsControls.ViewHandler.Mapper)
		{
			this.drawMapper = drawMapper ?? new DrawMapper<TViewDrawable, TVirtualView>(GraphicsControls.ViewHandler.DrawMapper);
		}


		public bool PointsContained(PointF[] points) => points.Any(p => Bounds.BoundsContains(p));

		protected PointF CurrentTouchPoint { get; set; }
		ControlState currentState = ControlState.Default;

		public ControlState CurrentState
		{
			get => VirtualView.IsEnabled ? currentState : ControlState.Disabled;
			set
			{
				if (currentState == value)
					return;
				currentState = value;
				Drawable.CurrentState = value;
				ControlStateChanged();
				Invalidate();
			}
		}
		TViewDrawable _drawable;
		protected TViewDrawable Drawable
		{
			get => _drawable ??= CreateDrawable();
			set => _drawable = value;
		}
		protected abstract TViewDrawable CreateDrawable();
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

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint) => Drawable.GetDesiredSize(VirtualView, widthConstraint, heightConstraint);

		public RectangleF Bounds { get; private set; }

		public bool TouchEnabled { get; set; } = true;
		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);
			Drawable.View = VirtualView;
			Invalidate();
		}
		public virtual void Draw(ICanvas canvas, RectangleF dirtyRect)
		{
			if (VirtualView == null || drawMapper == null)
				return;
			canvas.SaveState();
			var layers = LayerDrawingOrder();
			var rect = (VirtualView is IPadding padding) ? dirtyRect.ApplyPadding(padding.Padding) : dirtyRect;
			foreach (var layer in layers)
			{
				drawMapper.DrawLayer(canvas, rect, Drawable, VirtualView, layer);
			}

			//TODO: Bring back when we have ClipShape
			//var clipShape = VirtualView?.GetClipShape() ?? VirtualView?.GetBorder();
			//if (clipShape != null)
			//	canvas.ClipPath(clipShape.PathForBounds(rect).ToSKPath());

			//drawMapper.DrawLayer(canvas, rect, this, VirtualView, SkiaEnvironmentKeys.Border);
			canvas.ResetState();
		}

		public abstract string[] LayerDrawingOrder();

		DrawMapper IGraphicsControl.DrawMapper => drawMapper;


	}
}
