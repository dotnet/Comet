using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WPFListView = System.Windows.Controls.ListView;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.WPF.Handlers
{
    public class ListViewHandler : WPFListView, WPFViewHandler
    {
        public static readonly PropertyMapper<ListView> Mapper = new PropertyMapper<ListView>()
        {
            [nameof(Text.Value)] = MapListProperty
        };
        
        internal ListView listView;

        public ListViewHandler()
        {
            SelectionChanged += HandleSelectionChanged;
        }
        public new UIElement View => this;

        public object NativeView => View;

        public bool HasContainer
        {
            get => false;
            set { }
        }

        public void Remove(View view)
        {
        }

        public void SetView(View view)
        {
            listView = view as ListView;
            Mapper.UpdateProperties(this, listView);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, listView, property);
        }

        public static bool MapListProperty(IViewHandler viewHandler, ListView virtualView)
        {
            var nativeView = (ListViewHandler)viewHandler;
            foreach (var item in virtualView.List)
            {
                nativeView.Items.Add(new ListViewHandlerItem(nativeView, item));
            }
            return true;
        }

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listView?.OnSelected(SelectedIndex);
        }
    }

    public class ListViewHandlerItem : ListViewItem
    {
        public ListViewHandlerItem(ListViewHandler handler, object value)
        {
            var listView = handler.listView;
            var view = listView?.CellCreator?.Invoke(value);
            if (view != null)
                view.Parent = listView;
            Content = view?.ToEmbeddableView();
        }
    }
}