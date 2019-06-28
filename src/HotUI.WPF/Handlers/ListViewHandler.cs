using System;
using System.Collections.Generic;
using System.Windows;
using WPFListView = System.Windows.Controls.ListView;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.WPF.Handlers
{
    public class ListViewHandler : WPFListView, IUIElement
    {
        private static readonly PropertyMapper<ListView, ListViewHandler> Mapper = new PropertyMapper<ListView, ListViewHandler>(new Dictionary<string, Func<ListViewHandler, ListView, bool>>()
        {
        });
        
        private ListView _listView;

        public new UIElement View => this;
        
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