using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Comet.Android.Controls
{
    public class CometRecyclerViewHolder : RecyclerView.ViewHolder
    {
        private readonly IListView listView;

        public ViewGroup Parent { get; }
        public CometView CometView => (CometView) ItemView;
        
        public CometRecyclerViewHolder(
            ViewGroup parent, 
            IListView listView) : base(new CometView(parent.Context))
        {
            Parent = parent;
            this.listView = listView;

            CometView.Click += HandleClick;
        }

        private void HandleClick(object sender, EventArgs e)
        {
            listView?.OnSelected(0, this.AdapterPosition);
        }
    }
}