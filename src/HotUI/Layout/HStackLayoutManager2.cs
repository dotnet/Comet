using System;
using System.Collections.Generic;

namespace HotUI.Layout
{
    public class HStackLayoutManager2 : ILayoutManager2
    {
        public SizeF Measure(AbstractLayout layout, SizeF available)
        {
            var width = 0f;
            var height = 0f;
            var spacerCount = 0;
            var index = 0;

            foreach (var view in layout)
            {
                if (view is Spacer)
                {
                    spacerCount++;
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
            
            foreach (var view in layout)
            {
                if (view is Spacer)
                {
                    spacerCount++;
                    sizes.Add(new SizeF());
                }
                else
                {
                    var size = view.MeasuredSize;
                    sizes.Add(size);
                    height = Math.Max(size.Height, height);
                    nonSpacerWidth += size.Width + view.Padding.HorizontalThickness;
                }
                index++;
            }

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
                SizeF size;
                if (view is Spacer)
                {
                    size = new SizeF(spacerWidth, height);
                }
                else
                {
                    size = sizes[index];
                }

                var alignment = view.FrameConstraints?.Alignment ?? Alignment.Center;
                var alignedY = y;

                var padding = view.GetPadding();
                
                switch (alignment.Vertical)
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
                        break;
                    case VerticalAlignment.LastTextBaseline:
                        throw new NotSupportedException(VerticalAlignment.LastTextBaseline.ToString());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                if (padding != null)
                    x += padding.Left;
                
                view.Frame = new RectangleF(x,alignedY,size.Width, size.Height);
                x += size.Width;

                if (padding != null)
                    x += padding.Right;
                
                index++;
            }
        }
    }
}