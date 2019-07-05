using System;
using System.Collections.Generic;

namespace HotUI
{
    public class PropertyMapper<TVirtualView, TBaseNativeView, TNativeView> : Dictionary<string, Func<TNativeView, TVirtualView, bool>> where TVirtualView:View where TNativeView:TBaseNativeView
    {
        private readonly PropertyMapper<View, TBaseNativeView, TBaseNativeView> _chained;

        public PropertyMapper()
        {

        }

        public PropertyMapper(PropertyMapper<View, TBaseNativeView, TBaseNativeView> chained)
        {
            _chained = chained;
        }

        public void UpdateProperties( TNativeView nativeView, TVirtualView virtualView)
        {
            if (virtualView == null)
                return;

            if (_chained != null)
                _chained.UpdateProperties(nativeView, virtualView);
            
            foreach (var entry in this)
                entry.Value.Invoke(nativeView, virtualView);
        }
        
        public bool UpdateProperty( TNativeView nativeView, TVirtualView virtualView, string property)
        {
            if (virtualView == null)
                return false;
            
            if (TryGetValue(property, out var updater))
                return updater.Invoke(nativeView, virtualView);

            if (_chained != null)
                return _chained.UpdateProperty(nativeView, virtualView, property);

            return false;
        }
    }
}