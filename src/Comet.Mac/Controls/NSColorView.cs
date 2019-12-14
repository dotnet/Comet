using System;
using AppKit;
using CoreGraphics;
using Foundation;

namespace AppKit
{
	[Register("NSColorView")]
	public class NSColorView : NSView
	{
		public NSColorView()
		{
		}
		public NSColorView(IntPtr handle) : base(handle)
		{
		}

		public NSColorView(CGRect rect) : base(rect)
		{

		}

		NSColor backgroundColor = NSColor.Clear;
		public NSColor BackgroundColor
		{
			get
			{
				return backgroundColor;
			}
			set
			{
				backgroundColor = value;
				this.SetNeedsDisplayInRect(Bounds);
			}
		}

		public override bool IsFlipped => true;
		public override void DrawRect(CGRect dirtyRect)
		{
			var context = NSGraphicsContext.CurrentContext.GraphicsPort;
			context.SetFillColor(backgroundColor.CGColor); //White
			context.FillRect(dirtyRect);
			base.DrawRect(dirtyRect);
		}
	}
}

