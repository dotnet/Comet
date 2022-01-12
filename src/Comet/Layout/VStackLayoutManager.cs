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

		var measured = rect.Size;
		measured.Width -= padding.HorizontalThickness;
		measured.Height -= padding.VerticalThickness;

		double width = 0;

		var index = 0;
		double nonSpacerHeight = 0;
		var spacerCount = 0;
		var sizes = new List<Size>();
		var lastWasSpacer = false;

		foreach (var v in layout)
		{
			var isSpacer = false;

			if (v is Spacer)
			{
				spacerCount++;
				isSpacer = true;
				sizes.Add(new Size());
			}
			else if(v is View view)
			{
				var size = view.MeasuredSize;
				var constraints = view.GetFrameConstraints();
				var margin = view.GetMargin();
				var sizing = view.GetHorizontalLayoutAlignment(layout);

				if (constraints?.Width != null)
					size.Width = Math.Min((float)constraints.Width, measured.Width);

				if (constraints?.Height != null)
					size.Height = Math.Min((float)constraints.Height, measured.Height);

				if (sizing == Microsoft.Maui.Primitives.LayoutAlignment.Fill && constraints?.Width == null)
					size.Width = measured.Width - margin.HorizontalThickness;

				sizes.Add(size);
				width = Math.Max(size.Width, width);
				nonSpacerHeight += size.Height + margin.VerticalThickness;
			}

			if (index > 0 && !lastWasSpacer && !isSpacer)
				nonSpacerHeight += _spacing;

			lastWasSpacer = isSpacer;
			index++;
		}

		nonSpacerHeight = Math.Min(nonSpacerHeight, measured.Height);

		double spacerHeight = 0;
		if (spacerCount > 0)
		{
			var availableHeight = measured.Height - nonSpacerHeight;
			spacerHeight = availableHeight / spacerCount;
		}

		var x = rect.X + padding.Left;
		var y = rect.Y + padding.Top;
		index = 0;
		foreach (var v in layout)
		{
			var isSpacer = false;
			var view = (View)v;
			Size size;
			if (v is Spacer)
			{
				isSpacer = true;
				size = new Size(width, spacerHeight);
			}
			else
			{
				size = sizes[index];
			}

			var constraints = view.GetFrameConstraints();
			var alignment = constraints?.Alignment?.Horizontal ?? _defaultAlignment;
			var alignedX = x;

			var margin = view.GetMargin();

			switch (alignment)
			{
				case HorizontalAlignment.Center:
					alignedX += ((measured.Width - size.Width - margin.HorizontalThickness) / 2) + padding.Left;
					break;
				case HorizontalAlignment.Trailing:
					alignedX = layout.Frame.Width - size.Width - (margin.Right + padding.Right);
					break;
				case HorizontalAlignment.Leading:
					alignedX = margin.Left + padding.Left;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			if (index > 0 && !lastWasSpacer && !isSpacer)
				y += _spacing;

			y += margin.Top;

			var sizing = view.GetHorizontalLayoutAlignment(layout);
			if (sizing == LayoutAlignment.Fill && constraints?.Width == null)
			{
				alignedX = margin.Left + padding.Left;
				size.Width = measured.Width - margin.HorizontalThickness - padding.HorizontalThickness;
			}

			view.Frame = new Rectangle(alignedX, y, size.Width, size.Height);

			y += size.Height;
			y += margin.Bottom;

			lastWasSpacer = isSpacer;
			index++;
		}
		return new Size(width, y);
	}
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
		var spacerCount = 0;
		var lastWasSpacer = false;


		foreach (var view in layout)
		{
			var isSpacer = false;

			if (view is Spacer)
			{
				spacerCount++;
				isSpacer = true;

				if (!view.MeasurementValid)
				{
					view.MeasuredSize = new Size(-1, -1);
					view.MeasurementValid = true;
				}
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
				if (sizing == LayoutAlignment.Fill && constraints?.Width == null)
					width = widthConstraint;

				width = Math.Max(finalWidth, width);
				height += finalHeight;
			}

			if (index > 0 && !lastWasSpacer && !isSpacer)
				height += _spacing;

			lastWasSpacer = isSpacer;
			index++;
		}

		if (spacerCount > 0)
			height = heightConstraint;

		var layoutMargin = layout.GetMargin();

		if (layoutHorizontalSizing == LayoutAlignment.Fill)
			width = widthConstraint;

		if (layoutVerticalSizing == LayoutAlignment.Fill)
			height = heightConstraint - layoutMargin.VerticalThickness;

		width += padding.VerticalThickness;
		height += padding.HorizontalThickness;

		if (frameConstraints?.Height > 0 && frameConstraints?.Width > 0)
			return new Size(frameConstraints.Width.Value, frameConstraints.Height.Value);

		return new Size(width, height);
	}
}


