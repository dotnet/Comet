using System;
using System.Drawing;
using CoreGraphics;
using UIKit;

namespace Comet
{
	public static class UIViewExtensions
	{
		public static PointF[] GetPointsInView(this UIView target, UIEvent touchEvent)
		{
			var touchSet = touchEvent.TouchesForView(target);
			if (touchSet == null || touchSet.Count == 0)
				return new PointF[0];

			var touches = touchSet.ToArray<UITouch>();
			var points = new PointF[touches.Length];
			for (int i = 0; i < touches.Length; i++)
			{
				var touch = touches[i];
				var locationInView = touch.LocationInView(target);
				points[i] = new PointF((float)locationInView.X, (float)locationInView.Y);
			}

			return points;
		}

		public static UITextAlignment ToUITextAlignment(this TextAlignment? target)
		{
			if (target == null)
				return UITextAlignment.Natural;

			switch (target)
			{
				case TextAlignment.Natural:
					return UITextAlignment.Natural;
				case TextAlignment.Left:
					return UITextAlignment.Left;
				case TextAlignment.Right:
					return UITextAlignment.Right;
				case TextAlignment.Center:
					return UITextAlignment.Center;
				case TextAlignment.Justified:
					return UITextAlignment.Justified;
				default:
					throw new ArgumentOutOfRangeException(nameof(target), target, null);
			}
		}
	}
}
