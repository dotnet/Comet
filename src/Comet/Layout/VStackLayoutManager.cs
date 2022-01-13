using Microsoft.Maui.Primitives;

namespace Comet.Layout;

public class VStackLayoutManager : Microsoft.Maui.Layouts.ILayoutManager
{
	private readonly HorizontalAlignment _defaultAlignment;
	private readonly double _spacing;

	public VStackLayoutManager(ContainerView layout, HorizontalAlignment alignment = HorizontalAlignment.Center,
		double? spacing = null)
	{
		_defaultAlignment = alignment;
		_spacing = spacing ?? 4;
		this.layout = layout;
	}

	ContainerView layout;
	public Size ArrangeChildren(Rectangle rect)
	{
		var padding = layout.GetPadding();
		var layoutRect = rect.ApplyPadding(padding);
		double spacerHeight = (layoutRect.Height - childrenHeight) / spacerCount;
		foreach (var view in layout)
		{
			if (view is Spacer)
			{
				layoutRect.Y += spacerHeight;
				continue;
			}


			var size = view.MeasuredSize;
			layoutRect.Height = size.Height;
			view.SetFrameFromNativeView(layoutRect);
			layoutRect.Y = view.Frame.Bottom + _spacing;

		}
		return new Size(layoutRect.Left,layoutRect.Bottom);
	}

	int spacerCount;
	double childrenHeight;
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
		spacerCount = 0;


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
				var sizing = view.GetHorizontalLayoutAlignment(layout);
				if (sizing == LayoutAlignment.Fill && constraints?.Width == null && !double.IsInfinity(widthConstraint))
					width = widthConstraint;

				width = Math.Max(finalWidth, width);
				height += finalHeight;
			}

			if (index > 0)
				height += _spacing;
			index++;
		}
		if(spacerCount > 0)
			childrenHeight = height;
		if (spacerCount > 0)
			height = heightConstraint;

		var layoutMargin = layout.GetMargin();

		if (layoutHorizontalSizing == LayoutAlignment.Fill && !double.IsInfinity(widthConstraint))
			width = widthConstraint;

		if (layoutVerticalSizing == LayoutAlignment.Fill && !double.IsInfinity(heightConstraint))
			height = heightConstraint - layoutMargin.VerticalThickness;

		width += padding.VerticalThickness;
		height += padding.HorizontalThickness;
		if (frameConstraints?.Height > 0 && frameConstraints?.Width > 0)
		{
			return new Size(frameConstraints.Width.Value, frameConstraints.Height.Value);
		}

		return new Size(width, height);
	}
}


