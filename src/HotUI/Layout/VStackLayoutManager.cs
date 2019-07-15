using System;
using System.Collections.Generic;

namespace HotUI.Layout
{
    public class VStackLayoutManager<T> : ILayoutManager<T>
    {
        public SizeF Measure(ILayoutHandler<T> handler, T parentView, AbstractLayout layout, SizeF available)
        {
            var width = 0f;
            var height = 0f;
            var spacerCount = 0;
            var index = 0;

            foreach (var subview in handler.GetSubviews())
            {
                var view = layout[index];
                if (view is Spacer)
                {
                    spacerCount++;
                }
                else
                {
                    var size = handler.Measure(subview, available);
                    width = Math.Max(size.Width, width);
                    height += size.Height;
                }
                index++;
            }

            if (spacerCount > 0)
                height = available.Height;

            return new SizeF(width, height);
        }

        public void Layout(
            ILayoutHandler<T> handler, 
            T parentView, 
            AbstractLayout layout,
            SizeF measured)
        {
            var width = 0f;

            var index = 0;
            var nonSpacerHeight = 0f;
            var spacerCount = 0;
            List<SizeF> sizes = new List<SizeF>();

            foreach (var subview in handler.GetSubviews())
            {
                var view = layout[index];
                if (view is Spacer)
                {
                    spacerCount++;
                    sizes.Add(new SizeF());
                }
                else
                {
                    var size = handler.GetSize(subview);
                    sizes.Add(size);
                    width = Math.Max(size.Width, width);
                    nonSpacerHeight += size.Height;
                }
                index++;
            }

            var spacerHeight = 0f;
            if (spacerCount > 0)
            {
                var availableHeight = measured.Height - nonSpacerHeight;
                spacerHeight = availableHeight / spacerCount;
            }

            var x = 0f;
            var y = 0f;
            index = 0;
            foreach (var subview in handler.GetSubviews())
            {
                var view = layout[index];
                SizeF size;
                if (view is Spacer)
                {
                    size = new SizeF(width, spacerHeight);
                }
                else
                {
                    size = sizes[index];
                }

                handler.SetFrame(subview, x, y, size.Width, size.Height);
                y += size.Height;
                index++;
            }
        }
    }
}