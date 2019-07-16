using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HotUI {
	public static class ViewExtensions {

		public static ListView<T> OnSelected<T> (this ListView<T> listview, Action<T> selected)
		{
			listview.ItemSelected = (o) => selected?.Invoke ((T)o);
			return listview;
		}

		public static List<FieldInfo> GetFieldsWithAttribute (this object obj, Type attribute)
		{
			var type = obj.GetType ();
			var fields = type.GetFields (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).
				Where (x => Attribute.IsDefined (x, attribute)).ToList ();
			return fields;
		}

		
	}
}
