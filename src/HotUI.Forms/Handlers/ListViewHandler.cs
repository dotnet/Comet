using System;
using System.Collections;
using System.Collections.Specialized;
using FListView = Xamarin.Forms.ListView;
using HListView = HotUI.ListView;
using HView = HotUI.View;
namespace HotUI.Forms.Handlers
{
    public class ListViewHandler : FListView, FormsViewHandler
    {
        class HotViewCell : Xamarin.Forms.ViewCell
        {

            public HotViewCell()
            {
                View = new Xamarin.Forms.BoxView();
            }

            View currentView;
            protected override void OnBindingContextChanged()
            {
                if (BindingContext == null)
                {
                    View = new Xamarin.Forms.BoxView();
                }

                currentView = (BindingContext as Tuple<object, HotUI.View>)?.Item2;

                //TODO; implement something smart here to re-use the old view if possible.
                //View builders really are perfect for this. Maybe cell stuff should be wrapped in ViewBuilder
                if (View is IViewHandler iview)
                {
                    currentView.ViewHandler = iview;
                }
                View = currentView.ToForms();
            }
        }

        public ListViewHandler()
        {
            this.HasUnevenRows = true;
            this.ItemTemplate = new Xamarin.Forms.DataTemplate(typeof(HotViewCell));
            this.ItemSelected += ListViewHandler_ItemSelected;

        }

        private void ListViewHandler_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
            => listView?.OnSelected(0,(e.SelectedItem as Tuple<int, HotUI.View>).Item1);

        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;
        public Xamarin.Forms.View View => this;
        public object NativeView => View;
        public bool HasContainer { get; set; } = false;
        
        public SizeF Measure(SizeF availableSize)
        {
            return availableSize;
        }

        public void SetFrame(RectangleF frame)
        {
            // Do nothing
        }

        protected IListView listView;
        public void Remove(HView view)
        {
            //throw new NotImplementedException ();
        }
        HotListWrapper currentWrapper;
        public void SetView(HView view)
        {
            listView = view as HListView;
            this.ItemsSource = currentWrapper = new HotListWrapper(listView);
        }

        public void UpdateValue(string property, object value)
        {
            currentWrapper?.Reload();
        }

        public void Dispose()
        {
            this.ItemSelected -= ListViewHandler_ItemSelected;
        }

        class HotListWrapper : IList, INotifyCollectionChanged
        {
            private readonly IListView list;

            public HotListWrapper(IListView list)
            {
                this.list = list;
            }
            public object this[int index]
            {
                get => new Tuple<int, HotUI.View>(index, list.ViewFor(0,index));
                set => throw new NotImplementedException();
            }

            public bool IsFixedSize => true;

            public bool IsReadOnly => true;

            public int Count => list.Rows(0);

            public bool IsSynchronized => false;

            public object SyncRoot => throw new NotImplementedException();

            public event NotifyCollectionChangedEventHandler CollectionChanged;

            public void Reload()
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            public int Add(object value)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(object value)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            public IEnumerator GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public int IndexOf(object value)
            {
                var item = value as Tuple<int, HotUI.View>;
                return item.Item1;
            }

            public void Insert(int index, object value)
            {
                throw new NotImplementedException();
            }

            public void Remove(object value)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }
        }

    }
}
