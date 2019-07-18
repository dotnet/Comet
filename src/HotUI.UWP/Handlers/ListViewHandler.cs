using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWPListView = Windows.UI.Xaml.Controls.ListView;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.UWP.Handlers
{
    public class ListViewHandler : AbstractHandler<ListView, UWPListView>
    {
        public static readonly PropertyMapper<ListView> Mapper = new PropertyMapper<ListView>()
        {
            [nameof(HotUI.ListView.List)] = MapListProperty
        };

        public ListViewHandler() : base(Mapper)
        {

        }

        public ListView ListView => VirtualView;

        protected override UWPListView CreateView()
        {
            var listView = new UWPListView();
            listView.SelectionChanged += HandleSelectionChanged;
            return listView;
        }

        protected override void DisposeView(UWPListView listView)
        {
            listView.SelectionChanged -= HandleSelectionChanged;
        }

        public static void MapListProperty(IViewHandler viewHandler, ListView virtualView)
        {
            var nativeView = (UWPListView)viewHandler.NativeView;
            foreach (var item in virtualView.List)
            {
                nativeView.Items?.Add(new ListViewHandlerItem((ListViewHandler)viewHandler, item));
            }
        }

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VirtualView?.OnSelected(TypedNativeView.SelectedIndex);
        }
    }

    public class ListViewHandlerItem : ListViewItem
    {
        public ListViewHandlerItem(ListViewHandler handler, object value)
        {
            var listView = handler.TypedNativeView;
            var view = handler.ListView?.CellCreator?.Invoke(value);
            if (view != null)
                view.Parent = handler.ListView;
            Content = view?.ToView();
        }
    }
}