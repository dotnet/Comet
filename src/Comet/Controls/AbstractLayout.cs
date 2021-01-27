using System;
using System.Collections;
using System.Collections.Generic;
using System.Graphics;
using Comet.Layout;
using Xamarin.Platform;
using Xamarin.Platform.Layouts;

namespace Comet
{
	public abstract class AbstractLayout : ContainerView, ILayout
	{
		ILayoutManager layout;
		protected abstract ILayoutManager CreateLayoutManager();
		public ILayoutManager LayoutManager => layout ??= CreateLayoutManager();

		IReadOnlyList<IView> ILayout.Children => this.GetChildren();

		ILayoutHandler ILayout.LayoutHandler => throw new NotImplementedException();

		protected override void OnAdded(View view) { }// _layout?.Invalidate();

		protected override void OnClear() { }// _layout?.Invalidate();

		protected override void OnRemoved(View view) { }// _layout?.Invalidate();

		protected override void OnInsert(int index, View item) { }// _layout?.Invalidate();

		public override void LayoutSubviews(Rectangle frame)
		{
			base.LayoutSubviews(frame);
			var padding = this.GetPadding();
			var bounds = new Rectangle(
				padding.Left,
				padding.Right,
				frame.Width - padding.HorizontalThickness,
				frame.Height - padding.VerticalThickness);
			LayoutManager?.Arrange(bounds);
		}

		public override Size GetDesiredSize(Size availableSize)
		{
			if (IsMeasureValid)
				return MeasuredSize;
			MeasuredSize = LayoutManager.Measure(availableSize.Width, availableSize.Height);
			IsMeasureValid = true;
			return MeasuredSize;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			//LayoutManager?.Invalidate();
		}

		void ILayout.Add(IView child) => this.Add(child);
		void ILayout.Remove(IView child) => this.Remove(child);
	}
}
