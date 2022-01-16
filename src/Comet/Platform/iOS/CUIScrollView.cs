using System;
using CoreGraphics;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using UIKit;
namespace Comet.iOS
{
	public class CUIScrollView : UIScrollView
	{
		internal Action<Rectangle> CrossPlatformArrange { get; set; }
		CGRect rect;
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			if (rect == Frame)
				return;
			rect = Frame;
			var bounds = Frame.ToRectangle();
			CrossPlatformArrange?.Invoke(bounds);
		}
	}
}
