using System;
using Android.Views;
using Android.Widget;
using AView = Android.Views.View;
using AListView = Android.Widget.ListView;

namespace HotUI.Android.Handlers
{
    public class ListViewHandler : AListView, AndroidViewHandler
    {
        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        public ListViewHandler() : base(AndroidContext.CurrentContext)
        {
            this.Adapter = new ListViewAdapter();
            this.ItemSelected += ListViewHandler_ItemSelected;
            this.ItemClick += ListViewHandler_ItemClick;
        }

        private void ListViewHandler_ItemClick(object sender, ItemClickEventArgs e)
        {
            ((ListViewAdapter) this.Adapter).ListView?.OnSelected(e.Position);
        }

        private void ListViewHandler_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            ((ListViewAdapter) this.Adapter).ListView?.OnSelected(e.Position);
        }
        
        public AView View => this;
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

        public void Remove(View view)
        {
        }

        public void SetView(View view)
        {
            ((ListViewAdapter) this.Adapter).ListView = view as ListView;
        }

        public void UpdateValue(string property, object value)
        {
        }

        class ListViewAdapter : BaseAdapter<object>
        {
            public ListView ListView { get; set; }
            public override object this[int position] => ListView?.List?[position];

            public override int Count => ListView?.List?.Count ?? 0;

            public override long GetItemId(int position) => position;

            public override AView GetView(int position, AView convertView, ViewGroup parent)
            {
                var view = ListView?.CellCreator?.Invoke(ListView.List[position]);
                view.Parent = ListView;
                var cell = view.ToView();
                return cell;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            this.ItemSelected -= ListViewHandler_ItemSelected;
            this.ItemClick -= ListViewHandler_ItemClick;
            base.Dispose(disposing);
        }
    }
}