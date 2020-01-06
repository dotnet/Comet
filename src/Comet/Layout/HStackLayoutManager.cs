using System;
using System.Collections.Generic;
using System.Drawing;

namespace Comet.Layout
{
	public class HStackLayoutManager : ILayoutManager
	{
		private readonly VerticalAlignment _defaultAlignment;
		private readonly float _spacing;

		public HStackLayoutManager(
			VerticalAlignment alignment = VerticalAlignment.Center,
			float? spacing = null)
		{
			_defaultAlignment = alignment;
			_spacing = spacing ?? 4;
		}

		public void Invalidate()
		{

		}

		public SizeF Measure(AbstractLayout layout, SizeF available)
		{
			var index = 0;
			var width = 0f;
			var height = 0f;
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
						view.MeasuredSize = new SizeF(-1, -1);
						view.MeasurementValid = true;
					}
				}
				else
				{
					var size = view.MeasuredSize;
					if (!view.MeasurementValid)
					{
						view.MeasuredSize = size = view.Measure(available);
						view.MeasurementValid = true;
					}

					var finalHeight = size.Height;
					var finalWidth = size.Width;

					var margin = view.GetMargin();
					finalHeight += margin.VerticalThickness;
					finalWidth += margin.HorizontalThickness;

					var constraints = view.GetFrameConstraints();
					var verticalSizing = view.GetVerticalSizing(layout);
					if (verticalSizing == Sizing.Fill && constraints?.Height == null)
						height = available.Height;

					height = Math.Max(finalHeight, height);
					width += finalWidth;
				}

				if (index > 0 && !lastWasSpacer && !isSpacer)
					width += _spacing;

				lastWasSpacer = isSpacer;
				index++;
			}

			if (spacerCount > 0)
				width = available.Width;

			var layoutVerticalSizing = layout.GetVerticalSizing(layout);
			if (layoutVerticalSizing == Sizing.Fill)
				height = available.Height;

			var layoutHorizontalSizing = layout.GetHorizontalSizing(layout);
			if (layoutHorizontalSizing == Sizing.Fill)
				width = available.Width;

			return new SizeF(width, height);
		}

		public void Layout(AbstractLayout layout, RectangleF rect)
		{
			var measured = rect.Size;
			var height = 0f;

			var index = 0;
			var nonSpacerWidth = 0f;
			var spacerCount = 0;
			var sizes = new List<SizeF>();
			var lastWasSpacer = false;

			foreach (var view in layout)
			{
				var isSpacer = false;

				if (view is Spacer)
				{
					spacerCount++;
					isSpacer = true;
					sizes.Add(new SizeF());
				}
				else
				{
					var size = view.MeasuredSize;
					var constraints = view.GetFrameConstraints();
					var margin = view.GetMargin();
					var sizing = view.GetVerticalSizing(layout);

					// todo: this should never be needed.  Need to investigate this.
					if (!view.MeasurementValid)
					{
						view.MeasuredSize = size = view.Measure(measured);
						view.MeasurementValid = true;
					}

					if (constraints?.Width != null)
						size.Width = Math.Min((float)constraints.Width, measured.Width);

					if (constraints?.Height != null)
						size.Height = Math.Min((float)constraints.Height, measured.Height);

					if (sizing == Sizing.Fill && constraints?.Height == null)
						size.Height = measured.Height - margin.VerticalThickness;

					sizes.Add(size);
					height = Math.Max(size.Height, height);
					nonSpacerWidth += size.Width + margin.HorizontalThickness;
				}

				if (index > 0 && !lastWasSpacer && !isSpacer)
					nonSpacerWidth += _spacing;

				lastWasSpacer = isSpacer;
				index++;
			}

			nonSpacerWidth = Math.Min(nonSpacerWidth, measured.Width);

			var spacerWidth = 0f;
			if (spacerCount > 0)
			{
				var availableWidth = measured.Width - nonSpacerWidth;
				spacerWidth = availableWidth / spacerCount;
			}

			var x = rect.X;
			var y = rect.Y;
			index = 0;
			foreach (var view in layout)
			{
				var isSpacer = false;

				SizeF size;
				if (view is Spacer)
				{
					isSpacer = true;
					size = new SizeF(spacerWidth, height);
				}
				else
				{
					size = sizes[index];
				}

				var constraints = view.GetFrameConstraints();
				var alignment = constraints?.Alignment?.Vertical ?? _defaultAlignment;
				var alignedY = y;

				var margin = view.GetMargin();

				switch (alignment)
				{
					case VerticalAlignment.Center:
						alignedY += (measured.Height - size.Height - margin.Bottom + margin.Top) / 2;
						break;
					case VerticalAlignment.Bottom:
						alignedY += measured.Height - size.Height - margin.Bottom;
						break;
					case VerticalAlignment.Top:
						alignedY = margin.Top;
						break;
					case VerticalAlignment.FirstTextBaseline:
						throw new NotSupportedException(VerticalAlignment.FirstTextBaseline.ToString());
					case VerticalAlignment.LastTextBaseline:
						throw new NotSupportedException(VerticalAlignment.LastTextBaseline.ToString());
					default:
						throw new ArgumentOutOfRangeException();
				}

				if (index > 0 && !lastWasSpacer && !isSpacer)
					x += _spacing;

				x += margin.Left;

				var sizing = view.GetVerticalSizing(layout);
				if (sizing == Sizing.Fill && constraints?.Height == null)
				{
					alignedY = margin.Top;
					size.Height = measured.Height - margin.VerticalThickness;
				}

				view.Frame = new RectangleF(x, alignedY, size.Width, size.Height);

				x += size.Width;
				x += margin.Right;

				lastWasSpacer = isSpacer;
				index++;
			}
		}
	}
}
