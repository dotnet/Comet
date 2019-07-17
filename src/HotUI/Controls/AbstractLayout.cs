using System;
using System.Collections;
using System.Collections.Generic;

namespace HotUI
{
	public abstract class AbstractLayout : View, IList<View>, IContainerView
    {
		readonly List<View> _views = new List<View> ();

		public event EventHandler<LayoutEventArgs> ChildrenChanged;
		public event EventHandler<LayoutEventArgs> ChildrenAdded;
		public event EventHandler<LayoutEventArgs> ChildrenRemoved;

		public void Add (View view)
		{
			if (view == null)
				return;
			view.Parent = this;
            view.Navigation = Parent as NavigationView ?? Parent?.Navigation;
            _views.Add (view);
			ChildrenChanged?.Invoke (this, new LayoutEventArgs (_views.Count - 1, 1));
		}

		public void Clear ()
		{
			var count = _views.Count;
			if (count > 0)
            {
                var removed = new List<View>(_views);
                _views.Clear ();
				ChildrenRemoved?.Invoke (this, new LayoutEventArgs (0, count, removed));
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
			if (index >= 0)
            {
				item.Parent = null;
                item.Navigation = null;

                var removed = new List<View> { item };
                _views.Remove (item);
				ChildrenRemoved?.Invoke (this, new LayoutEventArgs (index, 1, removed));
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
            if (item == null)
                return;

			_views.Insert (index, item);
            item.Parent = this;
            item.Navigation = Parent as NavigationView ?? Parent?.Navigation;
            ChildrenAdded?.Invoke (this, new LayoutEventArgs (index, 1));
		}

		public void RemoveAt (int index)
        {
            if (index >= 0 && index < _views.Count)
            {
                var item = _views[index];
                item.Parent = null;
                item.Navigation = null;

                var removed = new List<View> { item };
                _views.RemoveAt(index);
                ChildrenRemoved?.Invoke(this, new LayoutEventArgs(index, 1, removed));
            }
        }

		public View this [int index]
        {
			get => _views [index];
			set
            {
                var item = _views[index];
                item.Parent = null;
                item.Navigation = null;
                var removed = new List<View> { item };

                _views[index] = value;

                value.Parent = null;
                value.Navigation = null;

                ChildrenChanged?.Invoke (this, new LayoutEventArgs (index, 1, removed));
			}
		}

		protected override void OnParentChange (View parent)
		{
			base.OnParentChange (parent);
			foreach (var view in _views)
            {
				view.Parent = this;
                view.Navigation = parent as NavigationView ?? parent?.Navigation;
            }
		}
		internal override void ContextPropertyChanged (string property, object value)
		{
			base.ContextPropertyChanged (property, value);
			foreach (var view in _views) {
				view.ContextPropertyChanged (property, value);
			}
		}
        protected override void Dispose(bool disposing)
        {
            foreach (var view in _views)
            {
                view.Dispose();
            }
            _views.Clear();
            base.Dispose(disposing);
        }
	}
}