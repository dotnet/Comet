using System;
using System.Collections.Generic;
using System.Linq;

namespace Comet
{
	public class PropertyMapper
	{
		internal Dictionary<string, Action<IViewHandler, View>> genericMap = new Dictionary<string, Action<IViewHandler, View>>();
	}
	public class PropertyMapper<TVirtualView> : PropertyMapper
		where TVirtualView : View
	{
		public ICollection<string> Keys => genericMap.Keys;
		public int Count => genericMap.Count;

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
			//Make a copy, since we are going to mess it up :D
			genericMap = new Dictionary<string, Action<IViewHandler, View>>(chained.genericMap);
		}

		public PropertyMapper(PropertyMapper defaultMapper, PropertyMapper instanceMapper)
		{
			//Make a copy, since we are going to mess it up :D
			genericMap = new Dictionary<string, Action<IViewHandler, View>>(defaultMapper.genericMap);
			//Overwrite the old values with the instance ones!
			foreach (var pair in instanceMapper.genericMap)
				genericMap[pair.Key] = pair.Value;
		}

		public void UpdateProperties(IViewHandler viewRenderer, TVirtualView virtualView)
		{
			if (virtualView == null)
				return;
			foreach (var key in genericMap.Keys)
			{
				UpdateProperty(key, viewRenderer, virtualView);
			}
		}

		protected void UpdateProperty(string key, IViewHandler viewRenderer, TVirtualView virtualView)
		{
			if (genericMap.TryGetValue(key, out var action))
				action?.Invoke(viewRenderer, virtualView);
		}

		public void UpdateProperty(IViewHandler viewRenderer, TVirtualView virtualView, string property)
		{
			if (virtualView == null)
				return;
			UpdateProperty(property, viewRenderer, virtualView);
		}


	}
}
