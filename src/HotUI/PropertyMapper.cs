using System;
using System.Collections.Generic;

namespace HotUI
{
    public class PropertyMapper<TVirtualView> : Dictionary<string, Func<IViewHandler, TVirtualView, bool>> 
        where TVirtualView:View 
    {
        private readonly PropertyMapper<View> _chained;

        public PropertyMapper()
        {
        }

        public PropertyMapper(PropertyMapper<View> chained) 
        {
            _chained = chained;
        }

        public void UpdateProperties( IViewHandler viewHandler, TVirtualView virtualView)
        {
            if (virtualView == null)
                return;
            
            foreach (var entry in this)
                entry.Value.Invoke(viewHandler, virtualView);
            
            _chained?.UpdateProperties(viewHandler, virtualView);
        }
        
        public bool UpdateProperty(IViewHandler viewHandler, TVirtualView virtualView, string property)
        {
            if (virtualView == null)
                return false;
            
            if (TryGetValue(property, out var updater))
                return updater.Invoke(viewHandler, virtualView);

            return _chained != null && _chained.UpdateProperty(viewHandler, virtualView, property);
        }
    }
}