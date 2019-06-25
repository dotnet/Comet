using System;
namespace HotUI {
	public static class ViewExtensions {

		public static ListView<T> OnSelected<T> (this ListView<T> listview, Action<T> selected)
		{
			listview.ItemSelected = (o) => selected?.Invoke ((T)o);
			return listview;
		}
		
	}
}
