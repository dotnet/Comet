using System;
using System.Collections.Generic;

namespace HotUI.Mac.Handlers
{
    public class PropertyMapper<TVirtualView, TNativeView>
    {
        private readonly Dictionary<string, Func<TNativeView, TVirtualView, bool>> _updaters;

        public PropertyMapper()
        {
            _updaters = new Dictionary<string, Func<TNativeView, TVirtualView, bool>>();
        }
        
        public PropertyMapper(Dictionary<string, Func<TNativeView, TVirtualView, bool>> updaters)
        {
            _updaters = updaters;
        }

        public void UpdateProperties( TNativeView nativeView, TVirtualView virtualView)
        {
            if (virtualView == null)
                return;
            
            foreach (var entry in _updaters)
                entry.Value.Invoke(nativeView, virtualView);
        }
        
        public bool UpdateProperty( TNativeView nativeView, TVirtualView virtualView, string property)
        {
            if (virtualView == null)
                return false;
            
            if (_updaters.TryGetValue(property, out var updater))
                return updater.Invoke(nativeView, virtualView);

            return false;
        }
    }
}