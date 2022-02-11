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
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			var bounds = Bounds.ToRectangle();
			CrossPlatformArrange?.Invoke(bounds);
		}
	}
}
