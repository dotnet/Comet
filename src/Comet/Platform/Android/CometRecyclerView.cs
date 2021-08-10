using Android.Content;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui;

namespace Comet.Android.Controls
{
	public class CometRecyclerView : RecyclerView
	{

		//private ListView listView;
		readonly CometRecyclerViewAdapter adapter;
		public CometRecyclerView(IMauiContext mauiContext) : base(mauiContext.Context)
		{
			var layoutManager = new LinearLayoutManager(mauiContext.Context);
			SetLayoutManager(layoutManager);
			SetAdapter(adapter =new CometRecyclerViewAdapter() { MauiContext = mauiContext});
			AddItemDecoration(new DividerItemDecoration(mauiContext.Context, layoutManager.Orientation));
		}

		public IListView ListView
		{
			get => adapter.ListView;
			set => adapter.ListView = value;
		}

		public void ReloadData() => adapter.NotifyDataSetChanged();
	}
}