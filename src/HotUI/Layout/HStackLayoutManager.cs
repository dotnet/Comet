using System;

namespace HotUI.Layout
{
    public class HStackLayoutManager<T> : ILayoutManager<T>
    {
        public void Layout(
            ILayoutHandler<T> handler, 
            T parentView, 
            AbstractLayout layout)
        {
            var x = 0f;
            var y = 0f;
            var height = 0f;
            
            foreach (var subview in handler.GetSubviews())
            {
                var size = handler.GetSize(subview);
                handler.SetFrame(subview,x,y,size.Width, size.Height);
                x += size.Width;
    
                height = Math.Max(size.Height, height);
            }
            
            handler.SetSize(parentView, x, height);
        }
    }
}