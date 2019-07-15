using System.Collections.Generic;
using HotUI.Graphics;

namespace HotUI
{
    public abstract class Shape : ContextualObject
    {        
        protected Shape()
        {
            
        }

        internal override void ContextPropertyChanged(string property, object value)
        {
            
        }

        public abstract PathF PathForBounds(RectangleF rect);
    }    
}