using System;
using System.Collections.Generic;

namespace Comet.Helpers
{
	public class WeakStack<T> where T : class
	{
		readonly Stack<WeakReference> items = new Stack<WeakReference>();

		public T Peek() => items.Count == 0 ? null : items.Peek()?.Target as T;

		public T Pop() => items.Count == 0 ? null : items.Pop()?.Target as T;

		public void Push(T value) => items.Push(new WeakReference(value));

		public int Count => items.Count;
	}
}
