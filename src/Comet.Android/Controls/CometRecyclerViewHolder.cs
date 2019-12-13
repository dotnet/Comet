using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Comet.Android.Controls
{
    public class CometRecyclerViewHolder : RecyclerView.ViewHolder
    {
        private readonly IListView listView;

        public FrameLayout Container { get; }
        public ViewGroup Parent { get; }

        public CometRecyclerViewHolder(FrameLayout itemView, ViewGroup parent, IListView listView)
            : base(itemView)
        {
            Container = itemView;
            Parent = parent;
            this.listView = listView;

            Container.Click += HandleClick;
        }

        private void HandleClick(object sender, EventArgs e)
        {
            listView?.OnSelected(0, this.AdapterPosition);
        }
    }
}