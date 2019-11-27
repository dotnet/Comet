using System;
using System.Collections.Generic;
using System.Linq;

namespace Comet.Internal
{
	public static class ListExtensions
	{
		public static T SafeGetAtIndex<T>(this IReadOnlyList<T> list, int index, Func<int, T> getItem = null)
		{
			if (index >= 0 && index < (list?.Count ?? 0))
				return list[index];
			if (getItem != null)
				return getItem(index);
			return default;

		}

		public static TValue GetOrCreateForKey<TValue, TKey>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
		{
			if (!dictionary.TryGetValue(key, out var result))
				dictionary[key] = result = new TValue();
			return result;
		}

		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) =>
			items.ToList().ForEach(action);
	}
}
