using System;
using Comet.Handlers;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.GraphicsControls
{
	public class ButtonHandler : GraphicsControlHandler<IButtonDrawable, IButton>
	{
		public ButtonHandler() :this(ViewHandler.DrawMapper, ViewHandler.Mapper)
		{

		}
		protected ButtonHandler(DrawMapper drawMapper, PropertyMapper mapper) : base(drawMapper, mapper)
		{

		}

		public override void EndInteraction(PointF[] points, bool inside)
		{
			if (inside)
				VirtualView?.Clicked();
			base.EndInteraction(points, inside);
		}

		protected override IButtonDrawable CreateDrawable() => new MaterialButtonHandler();
		public override string[] LayerDrawingOrder() => ViewHandler.DefaultLayerDrawingOrder;
	}
}
