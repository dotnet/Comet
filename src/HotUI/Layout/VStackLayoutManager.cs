using System;

namespace HotUI.Layout
{
    public class VStackLayoutManager<T> : ILayoutManager<T>
    {
        public Size Measure(ILayoutHandler<T> handler, T parentView, AbstractLayout layout, Size available)
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

            return new Size(width, height);
        }

        public void Layout(
            ILayoutHandler<T> handler, 
            T parentView, 
            AbstractLayout layout,
            Size measured)
        {
            var x = 0f;
            var y = 0f;
            var width = 0f;

            foreach (var subview in handler.GetSubviews())
            {
                var size = handler.GetSize(subview);
                handler.SetFrame(subview,x,y,size.Width, size.Height);
                y += size.Height;
    
                width = Math.Max(width, size.Width);
            }            
        }
    }
}