using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
namespace System.Collections.Generic {

    public class FixedSizedQueue<T> : IEnumerable<T>
    {
		private readonly object privateLockObject = new object ();

		readonly List<T> queue = new List<T> ();

		public int Count => queue.Count;

		public T this [int i] => queue[i];

		public int Size { get; private set; }

		public FixedSizedQueue (int size)
		{
			Size = size;
		}
		public Action<T> OnDequeue { get; set; }
		public void Enqueue (T obj)
		{
			queue.Add (obj);

			lock (privateLockObject) {
				while (queue.Count > Size) {
					T outObj = queue[0];
					queue.Remove (outObj);
					OnDequeue?.Invoke (outObj);
				}
			}
		}
		public bool Remove (T obj) 
		{
			bool removed = true;
			lock (privateLockObject)
			{
				if(queue.Contains(obj))
					removed = queue.Remove(obj);
			}

			OnDequeue?.Invoke(obj);
			return removed;
		}

		public void Clear ()
		{
			List<T> items;
			lock (privateLockObject)
			{
				items = queue.ToList();
			}
			items.ForEach((i)=>Remove(i));
		}
		public override string ToString ()
		{
			return string.Join (Environment.NewLine, queue.Select (x=> x).Reverse ());
		}
		public string ToString (Func<T, string> format)
		{
			return string.Join (Environment.NewLine, queue.Select (x => format (x)).Reverse ());
		}
		public int IndexOf (T obj) => queue.IndexOf (obj);

		public bool TryGetIndex (int index, out T value)
		{
			try {
				if (index < 0 || index > queue.Count - 1) {
					value = default (T);
					return false;
				}
				value = queue [index];
				return true;

			} catch {
				value = default (T);
				return false;
			}
			
		}

        public IEnumerator<T> GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
