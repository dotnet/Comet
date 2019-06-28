using System;
using System.Collections.Generic;
using UIKit;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.iOS
{
    public class ContentViewHandler : IUIView
    {
        private static readonly PropertyMapper<ContentView, ContentViewHandler> Mapper = new PropertyMapper<ContentView, ContentViewHandler>(new Dictionary<string, Func<ContentViewHandler, ContentView, bool>>
        {
            
        });
        
        private ContentView _contentView;

        public UIView View => _contentView?.Content?.ToView();

        public void Remove(View view)
        {
            _contentView = null;
        }

        public void SetView(View view)
        {
            _contentView = view as ContentView;
            Mapper.UpdateProperties(this, _contentView);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _contentView, property);
        }
    }
}