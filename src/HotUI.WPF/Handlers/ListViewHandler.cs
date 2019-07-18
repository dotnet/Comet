using System.Windows.Controls;
using WPFListView = System.Windows.Controls.ListView;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.WPF.Handlers
{
    public class ListViewHandler : AbstractControlHandler<ListView, WPFListView>
    {
        public static readonly PropertyMapper<ListView> Mapper = new PropertyMapper<ListView>()
        {
            [nameof(HotUI.ListView.List)] = MapListProperty
        };

        public ListViewHandler() : base(Mapper)
        {

        }

        public ListView ListView => VirtualView;

        protected override WPFListView CreateView()
        {
            var listView = new WPFListView();
            listView.SelectionChanged += HandleSelectionChanged;
            return listView;
        }

        protected override void DisposeView(WPFListView listView)
        {
            listView.SelectionChanged -= HandleSelectionChanged;
        }

        public static void MapListProperty(IViewHandler viewHandler, ListView virtualView)
        {
            var nativeView = (WPFListView) viewHandler.NativeView;
            foreach (var item in virtualView.List)
            {
                nativeView.Items?.Add(new ListViewHandlerItem((ListViewHandler) viewHandler, item));
            }
        }

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VirtualView?.OnSelected(TypedNativeView.SelectedIndex);
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
}