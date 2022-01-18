using System;
using Microsoft.Maui.Primitives;

namespace Comet.Layout
{
	public class HStackLayoutManager : Microsoft.Maui.Layouts.ILayoutManager
	{
		private readonly LayoutAlignment _defaultAlignment;
		private readonly float _spacing;

		public HStackLayoutManager(ContainerView layout,
			LayoutAlignment alignment = LayoutAlignment.Center,
			float? spacing = null)
		{
			this.layout = layout;
			_defaultAlignment = alignment;
			_spacing = spacing ?? 4;
		}

		ContainerView layout;
		public Size ArrangeChildren(Rectangle rect)
		{
			var layoutRect = rect;
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
				view.SetFrameFromNativeView(layoutRect,LayoutAlignment.Start, _defaultAlignment);
				layoutRect.X = view.Frame.Right + _spacing;
			}
			return new Size(layoutRect.Left, layoutRect.Bottom);
		}

		int spacerCount;
		double childrenWidth;
		public Size Measure(double widthConstraint, double heightConstraint)
		{
			//Lets adjust for Frame settings
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
					var size = view.Measure(widthConstraint, heightConstraint);

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


			
			return new Size(width, height);
		}
	}
}

