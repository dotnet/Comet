using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HotUI {

	public static class EnvironmentKeys {
		public const string DocumentsFolder = "DocumentsFolder";
		public const string UserFolder = "UserFolder";
		public const string OS = "OS";
	}

	class EnvironmentData : BindingObject {

		public View View { get; internal set; }

		protected ICollection<string> GetAllKeys ()
		{
			//This is the global Environment
			if (View?.Parent == null)
				return dictionary.Keys;

			//TODO: we need a fancy way of collapsing this. This may be too slow
			var keys = new HashSet<string> ();
			var localKeys = dictionary?.Keys;
			if (localKeys != null)
				foreach (var k in localKeys)
					keys.Add (k);

			var parentKeys = View?.Parent?.Context?.GetAllKeys () ?? View.Environment.GetAllKeys ();
			if (parentKeys != null)
				foreach (var k in parentKeys)
					keys.Add (k);
			return keys;
		}

		public T GetValue<T> (string key)
		{
			try {
				var value = GetValue (key);
				if (value == null)
					value = View?.Parent?.Context?.GetValue (key) ?? View.Environment.GetValue (key);
				return (T)value;
			} catch (Exception ex) {
				return default;
			}
		}

		public void SetValue (string key, object value) => SetProperty (value, key);
	}
}
