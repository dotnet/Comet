using System;
using System.Collections.Generic;
using System.Linq;

namespace Comet
{
    public class PropertyMapper<TVirtualView> : Dictionary<string, Action<IViewHandler, TVirtualView>>
        where TVirtualView : View
    {
        private readonly PropertyMapper<View> _chained;

        public PropertyMapper()
        {
        }

        public PropertyMapper(PropertyMapper<View> chained)
        {
            _chained = chained;
        }

        public void UpdateProperties(IViewHandler viewHandler, TVirtualView virtualView)
        {
            if (virtualView == null)
                return;
            var keys = _chained?.Keys?.Union(this.Keys) ?? Keys;
            foreach (var key in keys)
            {
                UpdateProperty(key, viewHandler, virtualView);
            }
        }

        protected void UpdateProperty(string key, IViewHandler viewHandler, TVirtualView virtualView)
        {
            if (this.TryGetValue(key, out var action))
                action?.Invoke(viewHandler, virtualView);
            else
                _chained?.UpdateProperty(key, viewHandler, virtualView);
        }

        public void UpdateProperty(IViewHandler viewHandler, TVirtualView virtualView, string property)
        {
            if (virtualView == null)
                return;

            if (TryGetValue(property, out var updater))
                updater.Invoke(viewHandler, virtualView);

            _chained?.UpdateProperty(viewHandler, virtualView, property);
        }
    }
}
