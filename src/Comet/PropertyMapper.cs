using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Comet
{
	public class PropertyMapper
	{
		internal Dictionary<string, Action<IViewHandler, View>> genericMap = new Dictionary<string, Action<IViewHandler, View>>();
		protected virtual void UpdateProperty(string key, IViewHandler viewRenderer, View virtualView)
		{
			if (genericMap.TryGetValue(key, out var action))
				action?.Invoke(viewRenderer, virtualView);
		}
		public void UpdateProperty(IViewHandler viewRenderer, View virtualView, string property)
		{
			if (virtualView == null)
				return;
			UpdateProperty(property, viewRenderer, virtualView);
		}
		public void UpdateProperties(IViewHandler viewRenderer, View virtualView)
		{
			if (virtualView == null)
				return;
			foreach (var key in Keys)
			{
				UpdateProperty(key, viewRenderer, virtualView);
			}
		}
		public virtual ICollection<string> Keys => genericMap.Keys;
	}

	public class PropertyMapper<TVirtualView> : PropertyMapper, IEnumerable
		where TVirtualView : View
	{
		private PropertyMapper chained;
		public PropertyMapper Chained
		{
			get => chained;
			set
			{
				chained = value;
				cachedKeys = null;
			}
		}

		ICollection<string> cachedKeys;
		public override ICollection<string> Keys => cachedKeys ??= (Chained?.Keys.Union(genericMap.Keys).ToList() as ICollection<string> ?? genericMap.Keys);

		public int Count => Keys.Count;

		public bool IsReadOnly => false;

		public Action<IViewHandler, TVirtualView> this[string key]
		{
			set => genericMap[key] = (r, v) => value?.Invoke(r, (TVirtualView)v);
		}

		public PropertyMapper()
		{
		}

		public PropertyMapper(PropertyMapper chained)
		{
			Chained = chained;
		}



		protected override void UpdateProperty(string key, IViewHandler viewRenderer, View virtualView)
		{
			if (genericMap.TryGetValue(key, out var action))
				action?.Invoke(viewRenderer, virtualView);
			else
				Chained?.UpdateProperty(viewRenderer, virtualView, key);
		}

		public void Add(string key, Action<IViewHandler, TVirtualView> action)
			=> this[key] = action;

		IEnumerator IEnumerable.GetEnumerator() => genericMap.GetEnumerator();

	}
}
