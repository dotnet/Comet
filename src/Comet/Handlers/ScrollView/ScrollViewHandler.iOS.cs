using System;
using Foundation;
using UIKit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using Comet.iOS;
using Microsoft.Maui.Graphics;
using CoreGraphics;

namespace Comet.Handlers
{
	public partial class ScrollViewHandler : ViewHandler<ScrollView, CUIScrollView>
	{

		private UIView _content;

		protected override CUIScrollView CreateNativeView() =>
			new CUIScrollView {
				ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Always,
				CrossPlatformArrange = Arange,
			};

		void Arange(Rectangle rect)
		{
			var measuredSize = VirtualView.Content.Measure(float.PositiveInfinity, float.PositiveInfinity);
			//Make sure we at least fit the scroll view
			measuredSize.Width = Math.Max(measuredSize.Width, rect.Width);
			measuredSize.Height = Math.Max(measuredSize.Height, rect.Height);

			NativeView.ContentSize = measuredSize.ToCGSize();
			_content.Frame = new CGRect(CGPoint.Empty, measuredSize);
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);


			_content = VirtualView?.Content?.ToNative(MauiContext);
			if (_content != null)
			{
				//_content.SizeToFit();
				NativeView.Add(_content);
			}

			if (VirtualView.Orientation == Orientation.Horizontal)
			{
				NativeView.ShowsVerticalScrollIndicator = false;
				NativeView.ShowsHorizontalScrollIndicator = true;
			}
			else
			{
				NativeView.ShowsVerticalScrollIndicator = true;
				NativeView.ShowsHorizontalScrollIndicator = false;
			}
		}
		
	}
}
