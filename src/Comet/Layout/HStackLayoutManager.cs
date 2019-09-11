using System;
using System.Collections.Generic;

namespace Comet.Layout
{
    public class HStackLayoutManager : ILayoutManager
    {
        private readonly VerticalAlignment _defaultAlignment;
        private readonly float _spacing;

        public HStackLayoutManager(
            VerticalAlignment alignment, 
            float? spacing)
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
                    
                    var padding = view.GetPadding();
                    finalHeight += padding.VerticalThickness;
                    finalWidth += padding.HorizontalThickness;

                    var verticalSizing = view.GetVerticalSizing();
                    if (verticalSizing == Sizing.Fill)
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
            
            var layoutVerticalSizing = layout.GetVerticalSizing();
            if (layoutVerticalSizing == Sizing.Fill)
                height = available.Height;
            
            var layoutHorizontalSizing = layout.GetHorizontalSizing();
            if (layoutHorizontalSizing == Sizing.Fill)
                width = available.Width;
            
            return new SizeF(width, height);
        }

        public void Layout(AbstractLayout layout, RectangleF bounds)
        {
            var measured = bounds.Size;
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
                    var padding = view.GetPadding();
                    var sizing = view.GetVerticalSizing();

                    if (!view.MeasurementValid)
                    {
                        view.MeasuredSize = size = view.Measure(measured);
                        view.MeasurementValid = true;
                    }

                    if (constraints?.Width != null)
                        size.Width = Math.Min((float)constraints.Width, measured.Width);
                    
                    if (constraints?.Height != null)
                        size.Height = Math.Min((float)constraints.Height, measured.Height);

                    if (sizing == Sizing.Fill)
                        size.Height = measured.Height - padding.VerticalThickness;
                    
                    sizes.Add(size);
                    height = Math.Max(size.Height, height);
                    nonSpacerWidth += size.Width + padding.HorizontalThickness;
                }

                if (index > 0 && !lastWasSpacer && !isSpacer)
                    nonSpacerWidth += _spacing;
                
                lastWasSpacer = isSpacer;
                index++;
            }

            nonSpacerWidth = Math.Min(nonSpacerWidth, measured.Width);
            
            var spacerWidth = 0f;
            if (spacerCount>0)
            {
                var availableWidth = measured.Width - nonSpacerWidth;
                spacerWidth = availableWidth / spacerCount;
            }

            var x = bounds.X;
            var y = bounds.Y;
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

                var padding = view.GetPadding();
                
                switch (alignment)
                {
                    case VerticalAlignment.Center:
                        alignedY += (measured.Height - size.Height - padding.Bottom + padding.Top) / 2;
                        break;
                    case VerticalAlignment.Bottom:
                        alignedY += measured.Height - size.Height - padding.Bottom;
                        break;
                    case VerticalAlignment.Top:
                        alignedY = padding.Top;
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
                
                x += padding.Left;

                var sizing = view.GetVerticalSizing();
                if (sizing == Sizing.Fill)
                {
                    alignedY = padding.Top;
                    size.Height = measured.Height - padding.VerticalThickness;
                }
                
                view.Frame = new RectangleF(x, alignedY, size.Width, size.Height);
                
                x += size.Width;
                x += padding.Right;
                
                lastWasSpacer = isSpacer;
                index++;
            }
        }
    }
}
