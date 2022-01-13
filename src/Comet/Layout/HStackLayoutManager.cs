using System;
using Microsoft.Maui.Primitives;

namespace Comet.Layout
{
	public class HStackLayoutManager : Microsoft.Maui.Layouts.ILayoutManager
	{
		private readonly VerticalAlignment _defaultAlignment;
		private readonly float _spacing;

		public HStackLayoutManager(ContainerView layout,
			VerticalAlignment alignment = VerticalAlignment.Center,
			float? spacing = null)
		{
			this.layout = layout;
			_defaultAlignment = alignment;
			_spacing = spacing ?? 4;
		}

		ContainerView layout;
		public Size ArrangeChildren(Rectangle rect)
		{
			var padding = layout.GetPadding();
			var layoutRect = rect.ApplyPadding(padding);
			double spacerWidth = (layoutRect.Width - childrenWidth) / spacerCount;

			foreach (var view in layout)
			{
				if (view is Spacer)
				{
					layoutRect.X += spacerWidth;
					continue;
				}

				var size = view.MeasuredSize;
				layoutRect.Width = size.Width;
				view.SetFrameFromNativeView(layoutRect);
				layoutRect.X = view.Frame.Right + _spacing;
			}
			return new Size(layoutRect.Left, layoutRect.Bottom);
		}

		int spacerCount;
		double childrenWidth;
		public Size Measure(double wConstraint, double hConstraint)
		{
			//Lets adjust for Frame settings
			var frameConstraints = layout.GetFrameConstraints();

			var layoutVerticalSizing = ((IView)layout).VerticalLayoutAlignment;
			var layoutHorizontalSizing = ((IView)layout).HorizontalLayoutAlignment;


			double widthConstraint = frameConstraints?.Width > 0 ? frameConstraints.Width.Value : wConstraint;
			double heightConstraint = frameConstraints?.Height > 0 ? frameConstraints.Height.Value : hConstraint;

			//Lets adjust for padding
			var padding = layout.GetPadding();
			widthConstraint -= padding.HorizontalThickness;
			heightConstraint -= padding.VerticalThickness;

			var index = 0;
			double width = 0;
			double height = 0;


			foreach (var view in layout)
			{
				if (view is Spacer)
				{
					spacerCount++;
					if (!view.MeasurementValid)
					{
						view.MeasuredSize = new Size(-1, -1);
						view.MeasurementValid = true;
					}
					continue;
				}
				else
				{
					var size = view.MeasuredSize;
					if (!view.MeasurementValid)
					{
						view.MeasuredSize = size = view.Measure(widthConstraint,heightConstraint);
						view.MeasurementValid = true;
					}

					var finalHeight = size.Height;
					var finalWidth = size.Width;

					var margin = view.GetMargin();
					finalHeight += margin.VerticalThickness;
					finalWidth += margin.HorizontalThickness;

					var constraints = view.GetFrameConstraints();
					var verticalSizing = view.GetVerticalLayoutAlignment(layout);
					if (verticalSizing == LayoutAlignment.Fill && constraints?.Height == null && !double.IsInfinity(heightConstraint))
						height = heightConstraint;

					height = Math.Max(finalHeight, height);
					width += finalWidth;
				}

				if (index > 0)
					width += _spacing;
				index++;
			}
			childrenWidth = width;
			if (spacerCount > 0)
				width = widthConstraint;

			if (layoutVerticalSizing == LayoutAlignment.Fill && !double.IsInfinity(heightConstraint))
				height = heightConstraint;
			if (layoutHorizontalSizing == LayoutAlignment.Fill && !double.IsInfinity(widthConstraint))
				width = widthConstraint;

			width += padding.VerticalThickness;
			height += padding.HorizontalThickness;

			if (frameConstraints?.Height > 0 && frameConstraints?.Width > 0)
				return new Size(frameConstraints.Width.Value, frameConstraints.Height.Value);

			return new Size(width, height);
		}
	}
}

