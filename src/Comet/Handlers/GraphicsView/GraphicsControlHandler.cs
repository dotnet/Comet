using System;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Comet.Handlers
{
	public abstract partial class GraphicsControlHandler<TViewHandler, TVirtualView> : IGraphicsControl
		where TVirtualView : class, IView
		where TViewHandler : class, IGraphicsControl, IViewHandler
	{
		protected readonly DrawMapper drawMapper;


		protected GraphicsControlHandler() : base(GraphicsControls.ViewHandler.Mapper)
		{
			drawMapper = GraphicsControls.ViewHandler.DrawMapper;
		}

		protected GraphicsControlHandler(DrawMapper drawMapper, PropertyMapper mapper) : base(mapper ?? GraphicsControls.ViewHandler.Mapper)
		{
			this.drawMapper = drawMapper ?? new DrawMapper<TViewHandler, TVirtualView>(GraphicsControls.ViewHandler.DrawMapper);
		}

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


		public RectangleF Bounds { get; private set; }

		public bool PointsContained(PointF[] points) => points.Any(p => Bounds.BoundsContains(p));

		public bool TouchEnabled { get; set; } = true;

		public virtual void Draw(ICanvas canvas, RectangleF dirtyRect)
		{
			if (VirtualView == null || drawMapper == null)
				return;
			canvas.SaveState();
			var layers = LayerDrawingOrder();
			var rect = (VirtualView is IPadding padding) ? dirtyRect.ApplyPadding(padding.Padding) : dirtyRect;
			foreach (var layer in layers)
			{
				drawMapper.DrawLayer(canvas, rect, this, VirtualView, layer);
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
