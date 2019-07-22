using System;
using System.Collections.Generic;

namespace HotUI.Layout
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
                    
                    var padding = view.Padding;
                    finalHeight += padding.VerticalThickness;
                    finalWidth += padding.HorizontalThickness;

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

            return new SizeF(width, height);
        }

        public void Layout(AbstractLayout layout, SizeF measured)
        {
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

                    if (view.FrameConstraints?.Width != null)
                        size.Width = Math.Min((float)view.FrameConstraints.Width, measured.Width);
                    
                    if (view.FrameConstraints?.Height != null)
                        size.Height = Math.Min((float)view.FrameConstraints.Height, measured.Height);

                    sizes.Add(size);
                    height = Math.Max(size.Height, height);
                    nonSpacerWidth += size.Width + view.Padding.HorizontalThickness;
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

            var x = 0f;
            var y = 0f;
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
                
                var alignment = view.FrameConstraints?.Alignment?.Vertical ?? _defaultAlignment;
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

                view.Frame = new RectangleF(x, alignedY, size.Width, size.Height);
                
                x += size.Width;
                x += padding.Right;
                
                lastWasSpacer = isSpacer;
                index++;
            }
        }
    }
}