using System;
using System.Collections;
using System.Collections.Generic;

namespace HotUI
{
	public abstract class AbstractLayout : View, IList<View>, IContainerView {
		readonly List<View> _views = new List<View> ();

		public event EventHandler<LayoutEventArgs> ChildrenChanged;
		public event EventHandler<LayoutEventArgs> ChildrenAdded;
		public event EventHandler<LayoutEventArgs> ChildrenRemoved;

		public void Add (View view)
		{
			if (view == null)
				return;
			view.Parent = this;
			_views.Add (view);
			ChildrenChanged?.Invoke (this, new LayoutEventArgs (_views.Count - 1, 1));
		}

		public void Clear ()
		{
			var count = _views.Count;
			if (count > 0) {
				_views.Clear ();
				ChildrenRemoved?.Invoke (this, new LayoutEventArgs (0, count));
			}
		}

		public bool Contains (View item) => _views.Contains (item);

		public void CopyTo (View [] array, int arrayIndex)
		{
			_views.CopyTo (array, arrayIndex);
			ChildrenAdded?.Invoke (this, new LayoutEventArgs (arrayIndex, array.Length));
		}

		public bool Remove (View item)
		{
			var index = _views.IndexOf (item);
			if (index >= 0) {
				item.Parent = null;
				_views.Remove (item);
				ChildrenRemoved?.Invoke (this, new LayoutEventArgs (index, 1));
				return true;
			}

			return false;
		}

		public int Count => _views.Count;

		public bool IsReadOnly => false;

		public IEnumerator<View> GetEnumerator () => _views.GetEnumerator ();

		IEnumerator IEnumerable.GetEnumerator () => _views.GetEnumerator ();

		public IReadOnlyList<View> GetChildren () => _views;

		public int IndexOf (View item) => _views.IndexOf (item);

		public void Insert (int index, View item)
		{
			_views.Insert (index, item);
			item.Parent = null;
			ChildrenAdded?.Invoke (this, new LayoutEventArgs (index, 1));
		}

		public void RemoveAt (int index)
		{
			_views.RemoveAt (index);
			ChildrenRemoved?.Invoke (this, new LayoutEventArgs (index, 1));
		}

		public View this [int index] {
			get => _views [index];
			set {
				_views [index] = value;
				ChildrenChanged?.Invoke (this, new LayoutEventArgs (index, 1));
			}
		}
		protected override void OnParentChange (View parent)
		{
			base.OnParentChange (parent);
			foreach (var view in _views) {
				view.Parent = this;
			}
		}
		internal override void ContextPropertyChanged (string property, object value)
		{
			base.ContextPropertyChanged (property, value);
			foreach (var view in _views) {
				view.ContextPropertyChanged (property, value);
			}
		}
		protected override void OnDisposing ()
		{
			base.OnDisposing ();
			foreach (var view in _views) {
				view.Dispose ();
			}
			_views.Clear ();
		}
	}
}