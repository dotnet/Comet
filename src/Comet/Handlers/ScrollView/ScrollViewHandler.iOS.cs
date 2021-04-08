using System;
using Foundation;
using UIKit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using Comet.iOS;
using Microsoft.Maui.Graphics;

namespace Comet.Handlers
{
	public partial class ScrollViewHandler : ViewHandler<ScrollView, UIScrollView>
	{
		public ScrollViewHandler() : base(ViewHandler.ViewMapper)
		{

		}

		private UIView _content;

		protected override UIScrollView CreateNativeView() 
			=> NativeView as UIScrollView ?? new UIScrollView
			{
				ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Always,
			};

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			_content = VirtualView?.View?.ToNative(MauiContext);
			if (_content != null)
			{
				_content.SizeToFit();
				NativeView.Add(_content);

				var measuredSize = VirtualView.View.Measure(float.PositiveInfinity, float.PositiveInfinity);
				NativeView.ContentSize = measuredSize.ToCGSize();
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
		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			var measuredSize = VirtualView.View.Measure(float.PositiveInfinity, float.PositiveInfinity);
			NativeView.ContentSize = measuredSize.ToCGSize();
			return new Size(widthConstraint, heightConstraint);
		}



	}
}
