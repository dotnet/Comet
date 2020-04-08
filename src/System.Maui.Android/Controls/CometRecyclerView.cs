using Android.Content;
using Android.Support.V7.Widget;

namespace System.Maui.Android.Controls
{
    public class MauiRecyclerView : RecyclerView
    {
        private ListView listView;
        
        public MauiRecyclerView(Context context) : base(context)
        {
            var layoutManager = new LinearLayoutManager(context);
            SetLayoutManager(layoutManager);
            SetAdapter(new MauiRecyclerViewAdapter());
            AddItemDecoration(new DividerItemDecoration(context, layoutManager.Orientation));
        }

        public IListView ListView
        {
            get => ((MauiRecyclerViewAdapter)GetAdapter()).ListView;
            set => ((MauiRecyclerViewAdapter)GetAdapter()).ListView = value;
        }

        public void ReloadData() => ((MauiRecyclerViewAdapter)GetAdapter()).NotifyDataSetChanged();
    }
}