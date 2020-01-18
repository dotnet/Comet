using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Comet.Helpers
{
	public class WeakList<T> : IList<T>
	{
		List<WeakReference> items = new List<WeakReference>();
		public T this[int index] { get => (T)items[index].Target; set => throw new NotImplementedException(); }

		public int Count => CleanseItems().Count;

		public bool IsReadOnly => false;

		public void Add(T item) => CleanseItems().Add(new WeakReference(item));

		public void Clear()
		{
			var views = CleanseItems().ToList();
			foreach (var item in views)
			{
				(item.Target as View)?.Dispose();
			}
			items.Clear();
		}

		public bool Contains(T item) => CleanseItems().Any(x => x.IsAlive && EqualityComparer<T>.Default.Equals(item, (T)x.Target));


		public void CopyTo(T[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<T> GetEnumerator() => CleanseItems().Select(x => (T)x.Target).GetEnumerator();

		public int IndexOf(T item)
		{ //=> CleanseItems().IndexOf(items.FirstOrDefault(x => x.IsAlive && EqualityComparer<T>.Default.Equals(item, (T)x.Target)));

			throw new NotImplementedException();
		}
		public void Insert(int index, T item)
		{
			throw new NotImplementedException();
		}

		public bool Remove(T item) => items.RemoveAll(x => EqualityComparer<T>.Default.Equals(item, (T)x.Target)) > 0;

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		List<WeakReference> CleanseItems()
		{
			items.RemoveAll(x => !x.IsAlive || ((x.Target as View)?.IsDisposed ?? false));
			return items;
		}
		public void ForEach(Action<T> action)
		{
			var items = CleanseItems().ToList();
			foreach (var item in items)
			{
				if (item.IsAlive)
					action?.Invoke((T)item.Target);
			}
		}
	}
}
