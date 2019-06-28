using System;
using System.Collections.Generic;

namespace HotUI
{
    public class PropertyMapper<TVirtualView, TNativeView> : Dictionary<string, Func<TNativeView, TVirtualView, bool>>
    {        

        public void UpdateProperties( TNativeView nativeView, TVirtualView virtualView)
        {
            if (virtualView == null)
                return;
            
            foreach (var entry in this)
                entry.Value.Invoke(nativeView, virtualView);
        }
        
        public bool UpdateProperty( TNativeView nativeView, TVirtualView virtualView, string property)
        {
            if (virtualView == null)
                return false;
            
            if (TryGetValue(property, out var updater))
                return updater.Invoke(nativeView, virtualView);

            return false;
        }
    }
}