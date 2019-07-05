using System;
using System.Collections.Generic;

namespace HotUI.Layout
{
    public class HStackLayoutManager<T> : ILayoutManager<T>
    {
        public Size Measure(
            ILayoutHandler<T> handler,
            T parentView,
            AbstractLayout layout,
            Size available)
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
                    height = Math.Max(size.Height, height);
                    width += size.Width;
                }
                index++;
            }

            if (spacerCount > 0)
                width = available.Width;

            return new Size(width, height);
        }

        public void Layout(
            ILayoutHandler<T> handler, 
            T parentView, 
            AbstractLayout layout,
            Size measured)
        {
            var height = 0f;
            
            var index = 0;
            var nonSpacerWidth = 0f;
            var spacerCount = 0;
            List<Size> sizes = new List<Size>();
            
            foreach (var subview in handler.GetSubviews())
            {
                var view = layout[index];
                if (view is Spacer)
                {
                    spacerCount++;
                    sizes.Add(new Size());
                }
                else
                {
                    var size = handler.GetSize(subview);
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
            foreach (var subview in handler.GetSubviews())
            {
                var view = layout[index];
                Size size;
                if (view is Spacer)
                {
                    size = new Size(spacerWidth, height);
                }
                else
                {
                    size = sizes[index];
                }
                
                handler.SetFrame(subview,x,y,size.Width, size.Height);
                x += size.Width;
                index++;
            }
        }
    }
}