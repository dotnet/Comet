using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Comet
{
	public abstract class AbstractLayout : ContainerView, ILayout
	{
		ILayoutManager layout;
		protected abstract ILayoutManager CreateLayoutManager();
		public ILayoutManager LayoutManager => layout ??= CreateLayoutManager();
		public ILayoutHandler LayoutHandler => ViewHandler as ILayoutHandler;

		IReadOnlyList<IView> IContainer.Children => this.GetChildren();

		protected override void OnAdded(View view)
		{
			LayoutHandler?.Add(view);
			InvalidateMeasurement();
		}

		protected override void OnClear(List<View> views)
		{
			views?.ForEach(x => LayoutHandler?.Remove(x));
			InvalidateMeasurement();
		}

		protected override void OnRemoved(View view)
		{
			LayoutHandler?.Remove(view);
			InvalidateMeasurement();
		}

		protected override void OnInsert(int index, View item) {
			LayoutHandler.Add(item);
			InvalidateMeasurement();
		}

		public override void LayoutSubviews(Rectangle frame)
		{
			base.LayoutSubviews(frame);
			var padding = this.GetPadding();
			var bounds = new Rectangle(
				padding.Left,
				padding.Right,
				frame.Width - padding.HorizontalThickness,
				frame.Height - padding.VerticalThickness);
			LayoutManager?.ArrangeChildren(bounds);
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
