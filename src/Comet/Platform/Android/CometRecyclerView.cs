using Android.Content;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui;

namespace Comet.Android.Controls
{
	public class CometRecyclerView : RecyclerView
	{
		private readonly IMauiContext mauiContext;

		//private ListView listView;
		CometRecyclerViewAdapter Adapter;
		public CometRecyclerView(IMauiContext mauiContext) : base(mauiContext.Context)
		{
			var layoutManager = new LinearLayoutManager(mauiContext.Context);
			SetLayoutManager(layoutManager);
			SetAdapter(Adapter =new CometRecyclerViewAdapter() { MauiContext = mauiContext});
			AddItemDecoration(new DividerItemDecoration(mauiContext.Context, layoutManager.Orientation));
			this.mauiContext = mauiContext;
		}

		public IListView ListView
		{
			get => Adapter.ListView;
			set => Adapter.ListView = value;
		}

		public void ReloadData() => Adapter.NotifyDataSetChanged();
	}
}