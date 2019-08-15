using System.Collections.Generic;
using Comet.Graphics;

namespace Comet
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
