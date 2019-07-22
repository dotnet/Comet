using System.Windows.Controls;
using WPFListView = System.Windows.Controls.ListView;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.WPF.Handlers
{
    public class ListViewHandler : AbstractControlHandler<ListView, WPFListView>
    {
        public static readonly PropertyMapper<ListView> Mapper = new PropertyMapper<ListView>()
        {
            ["List)"] = MapListProperty
        };

        public ListViewHandler() : base(Mapper)
        {

        }

        public IListView ListView => VirtualView;

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

        public static void MapListProperty(IViewHandler viewHandler, IListView virtualView)
        {
            var nativeView = (WPFListView) viewHandler.NativeView;
            var sections = virtualView?.Sections() ?? 0;
            for(var s = 0; s < sections; s++)
            {
                var section = virtualView?.HeaderFor(s);
                if(section != null)
                    nativeView.Items?.Add(new ListViewHandlerItem((ListViewHandler)viewHandler, section));

                var rows = virtualView.Rows(s);
                for(var r = 0; r < rows; r++)
                {
                    var v = virtualView.ViewFor(s, r);
                    nativeView.Items?.Add(new ListViewHandlerItem((ListViewHandler)viewHandler, v));
                }
                var footer = virtualView?.FooterFor(s);
                if(footer != null)
                    nativeView.Items?.Add(new ListViewHandlerItem((ListViewHandler)viewHandler, footer));
            }
        }

        private void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = VirtualView.ItemAt(0, TypedNativeView.SelectedIndex);
            VirtualView?.OnSelected(item);
        }

        public class ListViewHandlerItem : ListViewItem
        {
            public ListViewHandlerItem(ListViewHandler handler, View view)
            {
                RemoveViewHandlers(view);
                var nativeView = new HUIListCell(view);
                Content = nativeView;
            }

            private void RemoveViewHandlers(View view)
            {
                view.ViewHandler = null;
                if (view is AbstractLayout layout)
                    foreach (var subview in layout)
                        RemoveViewHandlers(subview);
            }
        }
    }
}