using System;
using System.Drawing;
using AppKit;
using Comet.Mac.Extensions;

namespace Comet.Mac.Handlers
{
	public class ScrollViewHandler : AbstractControlHandler<ScrollView, NSScrollView>
	{
		public static readonly PropertyMapper<ScrollView> Mapper = new PropertyMapper<ScrollView>(ViewHandler.Mapper)
		{

		};

		private NSView _content;

		public ScrollViewHandler() : base(Mapper)
		{
		}

		protected override NSScrollView CreateView()
		{
			return new NSScrollView();
		}

		protected override void DisposeView(NSScrollView nativeView)
		{

		}

		public override void Remove(View view)
		{
			if (VirtualView?.View != null)
				VirtualView.View.NeedsLayout -= HandleViewNeedsLayout;

			_content?.RemoveFromSuperview();
			_content = null;

			base.Remove(view);
		}

		public override void SetView(View view)
		{
			base.SetView(view);

			var scroll = VirtualView;
			_content = scroll?.View?.ToView();
			if (_content != null)
			{
				if (VirtualView?.View != null)
					VirtualView.View.NeedsLayout += HandleViewNeedsLayout;

				var measuredSize = VirtualView.View.Measure(new SizeF(float.PositiveInfinity, float.PositiveInfinity));
				if (_content.Bounds.Width <= 0 && _content.Bounds.Height <= 0)
					_content.Frame = new CoreGraphics.CGRect(0, 0, measuredSize.Width, measuredSize.Height);

				TypedNativeView.DocumentView = _content;
			}

			if (VirtualView.Orientation == Orientation.Horizontal)
			{
				TypedNativeView.HasVerticalScroller = false;
				TypedNativeView.HasHorizontalScroller = true;
				TypedNativeView.HorizontalScrollElasticity = NSScrollElasticity.Automatic;
			}
			else
			{
				TypedNativeView.HasVerticalScroller = true;
				TypedNativeView.HasHorizontalScroller = false;
			}
		}

		private void HandleViewNeedsLayout(object sender, EventArgs e)
		{
			_content.NeedsLayout = true;
		}
	}
}
