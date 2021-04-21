using System;
using Comet.Handlers;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.GraphicsControls
{
	public abstract class ButtonHandler : GraphicsControlHandler<ButtonHandler, IButton>
	{
		protected ButtonHandler(DrawMapper drawMapper, PropertyMapper mapper) : base(drawMapper, mapper)
		{

		}
		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			var size = GraphicsPlatform.CurrentService.GetStringSize(VirtualView.Text, VirtualView.Font.FontFamily, (float)VirtualView.Font.FontSize);
			return new Size(Math.Min(size.Width, widthConstraint), Math.Min(size.Height, heightConstraint));
		}

		public override void EndInteraction(PointF[] points, bool inside)
		{
			if (inside)
				VirtualView?.Clicked();
			base.EndInteraction(points, inside);
		}
	}
}
