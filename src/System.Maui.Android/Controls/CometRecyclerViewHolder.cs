using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace System.Maui.Android.Controls
{
    public class MauiRecyclerViewHolder : RecyclerView.ViewHolder
    {
        private readonly IListView listView;

        public ViewGroup Parent { get; }
        public MauiView MauiView => (MauiView) ItemView;
        
        public MauiRecyclerViewHolder(
            ViewGroup parent, 
            IListView listView) : base(new MauiView(parent.Context))
        {
            Parent = parent;
            this.listView = listView;

            MauiView.Click += HandleClick;
        }

        private void HandleClick(object sender, EventArgs e)
        {
            listView?.OnSelected(0, this.AdapterPosition);
        }
    }
}