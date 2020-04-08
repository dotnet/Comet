using System;
using AppKit;

namespace System.Maui.Mac.Extensions
{
	public static class NSViewExtensions
	{
		public static void InsertSubview(this NSView parent, NSView childView, int index)
		{
			if (parent == null || childView == null)
				return;

			var otherView = parent.Subviews[index];
			parent.AddSubview(childView, NSWindowOrderingMode.Below, otherView);
		}

		public static void SetNeedsLayout(this NSView parent)
		{
			if (parent == null)
				return;

			parent.NeedsLayout = true;
		}

		public static Color ToColor(this NSColor color)
		{
			if (color == null)
				return null;

			color = color.UsingColorSpace(NSColorSpace.DeviceRGB);
			return new Color((float)color.RedComponent, (float)color.GreenComponent, (float)color.BlueComponent, (float)color.AlphaComponent);
		}

		public static NSColor ToNSColor(this Color color)
		{
			if (color == null)
				return null;

			return NSColor.FromDeviceRgba(color.R, color.G, color.B, color.A);
		}

		public static NSTextAlignment ToNSTextAlignment(this TextAlignment? target)
		{
			if (target == null)
				return NSTextAlignment.Natural;

			switch (target)
			{
				case TextAlignment.Natural:
					return NSTextAlignment.Natural;
				case TextAlignment.Left:
					return NSTextAlignment.Left;
				case TextAlignment.Right:
					return NSTextAlignment.Right;
				case TextAlignment.Center:
					return NSTextAlignment.Center;
				case TextAlignment.Justified:
					return NSTextAlignment.Justified;
				default:
					throw new ArgumentOutOfRangeException(nameof(target), target, null);
			}
		}
	}
}
