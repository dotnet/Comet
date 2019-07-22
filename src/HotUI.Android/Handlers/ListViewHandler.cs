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
            //TODO: Account for Section
            ((ListViewAdapter) this.Adapter).ListView?.OnSelected(0,e.Position);
        }

        private void ListViewHandler_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            //TODO: Account for Section
            ((ListViewAdapter) this.Adapter).ListView?.OnSelected(0,e.Position);
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
            if(nameof(ListView.ReloadData) == property)
            {
                (this.Adapter as ListViewAdapter)?.NotifyDataSetChanged();
            }
        }

        class ListViewAdapter : BaseAdapter<object>
        {
            public IListView ListView { get; set; }
            //TODO: Account for Section
            public override object this[int position] => ListView?.ViewFor(0,position);

            //TODO: Account for Section
            public override int Count => ListView?.Rows(0) ?? 0;

            public override long GetItemId(int position) => position;

            public override AView GetView(int position, AView convertView, ViewGroup parent)
            {
                //TODO: Account for Section
                var view = ListView?.ViewFor(0,position);                
                var cell = view?.ToView();

                var displayMetrics = parent.Context.Resources.DisplayMetrics;
                var density = displayMetrics.Density;
              
                var scaledSize = new SizeF(parent.Width / density, parent.Height / density);
                var measuredSize = view.Measure(scaledSize);
                view.MeasuredSize = measuredSize;
                view.MeasurementValid = true;

                cell.LayoutParameters = new ViewGroup.LayoutParams(parent.Width, (int)(measuredSize.Height* density));
                cell.SetMinimumHeight((int)(measuredSize.Height * density));

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