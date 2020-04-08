using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Maui.Internal
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

		public static bool TryRemove<T>(this IList<T> list, T item)
		{
			try
			{
				list.Remove(item);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static IList<T> InsertAfter<T>(this IList<T> list, T itemToAdd, T previousItem)
		{
			var index = list.IndexOf(previousItem);
			list.Insert(index + 1, itemToAdd);
			return list;
		}

		public static IList<T> InsertAfter<T>(this IList<T> list, IEnumerable<T> itemsToAdd, T previousItem)
		{
			var index = list.IndexOf(previousItem) + 1;
			foreach (var item in itemsToAdd)
				list.Insert(index++, item);
			return list;
		}
	}
}
