using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using UWPListView = Windows.UI.Xaml.Controls.ListView;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.UWP.Handlers
{
    public class ListViewHandler : UWPListView, IUIElement
    {
        private static readonly PropertyMapper<ListView, ListViewHandler> Mapper = new PropertyMapper<ListView, ListViewHandler>(new Dictionary<string, Func<ListViewHandler, ListView, bool>>()
        {
        });
        
        private ListView _listView;

        public UIElement View => this;
        
        public void Remove(View view)
        {
        }

        public void SetView(View view)
        {
            _listView = view as ListView;
            Mapper.UpdateProperties(this, _listView);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _listView, property);
        }
    }
}