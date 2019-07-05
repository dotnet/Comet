using System;
using System.Collections.Generic;
using System.Drawing;

namespace HotUI.Layout
{
    public class HStackLayoutManager<T> : ILayoutManager<T>
    {
        public void Layout(
            ILayoutHandler<T> handler, 
            T parentView, 
            AbstractLayout layout)
        {
            var height = 0f;
            
            var index = 0;
            var nonSpacerWidth = 0f;
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
                    height = Math.Max(size.Height, height);
                    nonSpacerWidth += size.Width;
                    index++;
                }
            }

            var spacerWidth = 0f;
            if (spacerCount>0)
            {
                var parentSize = handler.GetAvailableSize();
                var availableWidth = parentSize.Width - nonSpacerWidth;
                spacerWidth = availableWidth / spacerCount;
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
                    size = new SizeF(spacerWidth, height);
                }
                else
                {
                    size = sizes[index];
                    index++; 
                }
                
                handler.SetFrame(subview,x,y,size.Width, size.Height);
                x += size.Width;
            }


            handler.SetSize(parentView, x, height);
        }
    }
}