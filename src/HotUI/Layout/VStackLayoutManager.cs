using System;

namespace HotUI.Layout
{
    public class VStackLayoutManager<T> : ILayoutManager<T>
    {
        public void Layout(
            ILayoutHandler<T> handler, 
            T parentView, 
            AbstractLayout layout)
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
            
            handler.SetSize(parentView, width, y);
        }
    }
}