using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Primitives;

namespace Comet
{
	public abstract class AbstractLayout : ContainerView, ILayout
	{
		ILayoutManager layout;
		protected abstract ILayoutManager CreateLayoutManager();
		public ILayoutManager LayoutManager => layout ??= CreateLayoutManager();
		public ILayoutHandler LayoutHandler => ViewHandler as ILayoutHandler;

		Thickness IPadding.Padding => GetDefaultPadding();

		bool ILayout.ClipsToBounds { get; }

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

		protected virtual Thickness GetDefaultPadding() => this.GetEnvironment<Thickness>(nameof(Styles.Style.LayoutPadding));

		Size lastMeasureSize;
		public override Size GetDesiredSize(Size availableSize)
		{
			if (this.IsMeasureValid && availableSize == lastMeasureSize)
				return MeasuredSize;
			lastMeasureSize = availableSize;
			var frameConstraints = this.GetFrameConstraints();

			var layoutVerticalSizing = ((IView)this).VerticalLayoutAlignment;
			var layoutHorizontalSizing = ((IView)this).HorizontalLayoutAlignment;


			double widthConstraint = frameConstraints?.Width > 0 ? frameConstraints.Width.Value : availableSize.Width;
			double heightConstraint = frameConstraints?.Height > 0 ? frameConstraints.Height.Value : availableSize.Height;

			//Lets adjust for padding

			var padding = this.GetPadding(GetDefaultPadding());
			if (!double.IsInfinity(widthConstraint))
				widthConstraint -= padding.HorizontalThickness;
			if (!double.IsInfinity(heightConstraint))
				heightConstraint -= padding.VerticalThickness;


			var measured = LayoutManager.Measure(widthConstraint, heightConstraint);


			if (frameConstraints?.Height > 0 && frameConstraints?.Width > 0)
			{
				measured = new Size(frameConstraints.Width.Value, frameConstraints.Height.Value);
			}
			else
			{
				if (layoutVerticalSizing == LayoutAlignment.Fill && !double.IsInfinity(heightConstraint))
					measured.Height = heightConstraint;
				if (layoutHorizontalSizing == LayoutAlignment.Fill && !double.IsInfinity(widthConstraint))
					measured.Width = widthConstraint;

				if (!double.IsInfinity(measured.Width))
					measured.Width += padding.HorizontalThickness;
				if (!double.IsInfinity(measured.Height))
					measured.Height += padding.VerticalThickness;
			}

			var margin = this.GetMargin();
			if (!double.IsInfinity(measured.Width))
				measured.Width += margin.HorizontalThickness;
			if (!double.IsInfinity(measured.Height))
				measured.Height += margin.VerticalThickness;
			return MeasuredSize = measured;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			//LayoutManager?.Invalidate();
		}

		Rect lastRect;
		public virtual Size CrossPlatformMeasure(double widthConstraint, double heightConstraint) => GetDesiredSize(new Size(widthConstraint,heightConstraint));
		public virtual Size CrossPlatformArrange(Rect bounds) {
			if(bounds != lastRect)
			{
				Measure(bounds.Width,bounds.Height);
			}
			lastRect = bounds;
			var padding = this.GetPadding(GetDefaultPadding()); ;
			var b = bounds.ApplyPadding(padding);
			LayoutManager?.ArrangeChildren(b);
			return this.MeasuredSize;
		}
	}
}
