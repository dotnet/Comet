using System;
using AView = Android.Views.View;
using AListView = Android.Widget.ListView;
using Java.Lang;
using Android.Widget;
using Android.Views;

namespace HotUI.Android {
	public class ListViewHandler : AListView, IView {
		public ListViewHandler () : base (AndroidContext.CurrentContext)
		{
			this.Adapter = new ListViewAdapter ();
			this.ItemSelected += ListViewHandler_ItemSelected;
			this.ItemClick += ListViewHandler_ItemClick;
		}

		private void ListViewHandler_ItemClick (object sender, ItemClickEventArgs e)
		{
			((ListViewAdapter)this.Adapter).ListView?.OnSelected (e.Position);
		}

		private void ListViewHandler_ItemSelected (object sender, ItemSelectedEventArgs e)
		{
			((ListViewAdapter)this.Adapter).ListView?.OnSelected (e.Position);
		}

		public AView View => this;

		public void Remove (View view)
		{
		}

		public void SetView (View view)
		{
			((ListViewAdapter)this.Adapter).ListView = view as ListView;
		}

		public void UpdateValue (string property, object value)
		{

		}
		class ListViewAdapter : BaseAdapter<object> {

			public ListView ListView { get; set; }
			public override object this [int position] => ListView?.List? [position];

			public override int Count => ListView?.List?.Count ?? 0;

			public override long GetItemId (int position) => position;

			public override AView GetView (int position, AView convertView, ViewGroup parent) =>
				ListView?.CellCreator?.Invoke (ListView.List [position]).ToView ();

		}
	}
}
