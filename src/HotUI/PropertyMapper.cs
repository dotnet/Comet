using System;
using System.Collections.Generic;

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

            foreach (var entry in this)
                entry.Value.Invoke(viewHandler, virtualView);

            _chained?.UpdateProperties(viewHandler, virtualView);
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