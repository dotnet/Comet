using Android.Content;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui;

namespace Comet.Android.Controls
{
	public class CometRecyclerView : RecyclerView
	{
		private ListView listView;

		public CometRecyclerView(IMauiContext mauiContext) : base(mauiContext.Context)
		{
			var layoutManager = new LinearLayoutManager(mauiContext.Context);
			SetLayoutManager(layoutManager);
			SetAdapter(new CometRecyclerViewAdapter() { MauiContext = mauiContext});
			AddItemDecoration(new DividerItemDecoration(mauiContext.Context, layoutManager.Orientation));
		}

		public IListView ListView
		{
			get => ((CometRecyclerViewAdapter)GetAdapter()).ListView;
			set => ((CometRecyclerViewAdapter)GetAdapter()).ListView = value;
		}

		public void ReloadData() => ((CometRecyclerViewAdapter)GetAdapter()).NotifyDataSetChanged();
	}
}