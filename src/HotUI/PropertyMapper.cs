using System;
using System.Collections.Generic;

namespace HotUI
{
    public class PropertyMapper<TVirtualView, TBaseNativeView, TNativeView> : Dictionary<string, Func<TNativeView, TVirtualView, bool>> where TVirtualView:View where TBaseNativeView:class
    {
        private readonly PropertyMapper<View, TBaseNativeView, TBaseNativeView> _chained;
        private readonly Func<TNativeView, TBaseNativeView> _toBase;

        public PropertyMapper()
        {
        }

        public PropertyMapper(PropertyMapper<View, TBaseNativeView, TBaseNativeView> chained) 
        {
            _chained = chained;
        }

        public PropertyMapper(PropertyMapper<View, TBaseNativeView, TBaseNativeView> chained, Func<TNativeView, TBaseNativeView> getNative)
        {
            _chained = chained;
            _toBase = getNative;
        }

        public void UpdateProperties( TNativeView nativeView, TVirtualView virtualView)
        {
            if (virtualView == null)
                return;

            if (_chained != null)
            {
                TBaseNativeView native = _toBase?.Invoke(nativeView) ?? nativeView as TBaseNativeView;
                _chained.UpdateProperties(native, virtualView);
            }

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
            {
                TBaseNativeView native = _toBase?.Invoke(nativeView) ?? nativeView as TBaseNativeView;
                return _chained.UpdateProperty(native, virtualView, property);
            }

            return false;
        }
    }
}