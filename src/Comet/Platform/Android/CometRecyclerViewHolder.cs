using System;
using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Widget;
using Microsoft.Maui;
namespace Comet.Android.Controls
{
	public class CometRecyclerViewHolder : RecyclerView.ViewHolder
	{
		private readonly IListView listView;
		public IMauiContext MauiContext {get;set;}
		public ViewGroup Parent { get; }
		public CometView CometView => (CometView)ItemView;

		public CometRecyclerViewHolder(
			ViewGroup parent,
			IListView listView, IMauiContext mauiContext) : base(new CometView(mauiContext))
		{
			MauiContext  = MauiContext;
			Parent = parent;
			this.listView = listView;

			CometView.Click += HandleClick;
		}

		private void HandleClick(object sender, EventArgs e) => listView?.OnSelected(0, AdapterPosition);
	}
}