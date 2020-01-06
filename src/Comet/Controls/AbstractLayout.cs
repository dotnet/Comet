using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Comet.Layout;

namespace Comet
{
	public abstract class AbstractLayout : ContainerView
	{
		private readonly ILayoutManager _layout;

		protected AbstractLayout(ILayoutManager layoutManager)
		{
			_layout = layoutManager;
		}

		public ILayoutManager LayoutManager => _layout;

		protected override void OnAdded(View view) => _layout?.Invalidate();

		protected override void OnClear() => _layout?.Invalidate();

		protected override void OnRemoved(View view) => _layout?.Invalidate();

		protected override void OnInsert(int index, View item) => _layout?.Invalidate();

		public override void LayoutSubviews(RectangleF frame)
		{
			var padding = this.GetPadding();
			var bounds = new RectangleF(
				padding.Left,
				padding.Right,
				frame.Width - padding.HorizontalThickness,
				frame.Height - padding.VerticalThickness);
			_layout?.Layout(this, bounds);
		}

		public override SizeF GetIntrinsicSize(SizeF availableSize) => _layout.Measure(this, availableSize);

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			_layout?.Invalidate();
		}
	}
}
