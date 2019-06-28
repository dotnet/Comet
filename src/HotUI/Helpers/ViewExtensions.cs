using System;
using System.Collections.Generic;

namespace HotUI {
	public static class ViewExtensions {

		public static ListView<T> OnSelected<T> (this ListView<T> listview, Action<T> selected)
		{
			listview.ItemSelected = (o) => selected?.Invoke ((T)o);
			return listview;
		}

		public static T SetEnvironment<T>(this T view, string key, object value) where T: View
		{
			view.Context.SetValue (key, value);
			return view;
		}

		public static T SetEnvironment<T> (this T view, IDictionary<string, object> data) where T : View
		{
			foreach (var pair in data)
				view.Context.SetValue (pair.Key, pair.Value);
			return view;
		}

		public static T GetEnvironment<T> (this View view, string key) => view.Context.GetValue<T> (key);

	}
}
