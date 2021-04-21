using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native;
using Comet.GraphicsControls;
namespace Comet
{
	public class NativeGraphicsControlView : NativeGraphicsView
	{
		public NativeGraphicsControlView()
		{
			this.BackgroundColor = UIColor.Clear;
		}
		IGraphicsControl graphicsControl;
		public IGraphicsControl GraphicsControl {
			get => graphicsControl;
			set => Drawable = graphicsControl = value;
		}
		bool pressedContained;
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			try
			{
				if (!this.IsFirstResponder)
					this.BecomeFirstResponder();
				var viewPoints = this.GetPointsInView(evt);
				GraphicsControl?.StartInteraction(viewPoints);
				pressedContained = true;
			}
			catch (Exception exc)
			{
				//Logger.Warn("An unexpected error occured handling a touch event within the control.", exc);
			}
		}
		public override bool PointInside(CGPoint point, UIEvent uievent) => (GraphicsControl?.TouchEnabled ?? false) && base.PointInside(point, uievent);

		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			try
			{
				var viewPoints = this.GetPointsInView(evt);
				pressedContained = GraphicsControl?.PointsContained(viewPoints) ?? false;
				GraphicsControl?.DragInteraction(viewPoints);
			}
			catch (Exception exc)
			{
				//Logger.Warn("An unexpected error occured handling a touch moved event within the control.", exc);
			}
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			try
			{
				var viewPoints = this.GetPointsInView(evt);
				GraphicsControl?.EndInteraction(viewPoints, pressedContained);
			}
			catch (Exception exc)
			{
				//Logger.Warn("An unexpected error occured handling a touch ended event within the control.", exc);
			}
		}

		public override void TouchesCancelled(NSSet touches, UIEvent evt)
		{
			try
			{
				pressedContained = false;
				GraphicsControl?.CancelInteraction();
			}
			catch (Exception exc)
			{
				//Logger.Warn("An unexpected error occured cancelling the touches within the control.", exc);
			}
		}
	}
}
