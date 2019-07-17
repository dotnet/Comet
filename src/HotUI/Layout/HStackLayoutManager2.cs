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

                    height = Math.Max(size.Height, height);
                    width += size.Width;
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
                    nonSpacerWidth += size.Width;
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
                switch (alignment.Vertical)
                {
                    case VerticalAlignment.Center:
                        alignedY += (measured.Height - size.Height) / 2;
                        break;
                    case VerticalAlignment.Bottom:
                        alignedY += (measured.Height - size.Height);
                        break;
                    case VerticalAlignment.Top:
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
                
                view.Frame = new RectangleF(x,alignedY,size.Width, size.Height);
                x += size.Width;
                index++;
            }
        }
    }
}