using Android.Content;
using Android.Support.V7.Widget;

namespace Comet.Android.Controls
{
    public class CometRecyclerView : RecyclerView
    {
        private ListView listView;
        
        public CometRecyclerView(Context context) : base(context)
        {
            var layoutManager = new LinearLayoutManager(context);
            SetLayoutManager(layoutManager);
            SetAdapter(new CometRecyclerViewAdapter());
            AddItemDecoration(new DividerItemDecoration(context, layoutManager.Orientation));
        }

        public IListView ListView
        {
            get => ((CometRecyclerViewAdapter)GetAdapter()).ListView;
            set => ((CometRecyclerViewAdapter)GetAdapter()).ListView = value;
        }

        public void ReloadData() => ((CometRecyclerViewAdapter)GetAdapter()).NotifyDataSetChanged();
    }
}