using System;
using System;
using System.Collections;
using System.Collections.Generic;
namespace HotUI
{
    public class ContainerView : View, IList<View>, IContainerView
    {
        readonly protected List<View> Views = new List<View>();

        public event EventHandler<LayoutEventArgs> ChildrenChanged;
        public event EventHandler<LayoutEventArgs> ChildrenAdded;
        public event EventHandler<LayoutEventArgs> ChildrenRemoved;


        public void Add(View view)
        {
            if (view == null)
                return;
            view.Parent = this;
            view.Navigation = Parent as NavigationView ?? Parent?.Navigation;
            Views.Add(view);
            OnAdded(view);
            ChildrenChanged?.Invoke(this, new LayoutEventArgs(Views.Count - 1, 1));
        }

        protected virtual void OnAdded(View view)
        {

        }

        public void Clear()
        {
            var count = Views.Count;
            if (count > 0)
            {
                var removed = new List<View>(Views);
                Views.Clear();
                OnClear();
                ChildrenRemoved?.Invoke(this, new LayoutEventArgs(0, count, removed));
            }
        }

        protected virtual void OnClear()
        {

        }

        public bool Contains(View item) => Views.Contains(item);

        public void CopyTo(View[] array, int arrayIndex)
        {
            Views.CopyTo(array, arrayIndex);

            ChildrenAdded?.Invoke(this, new LayoutEventArgs(arrayIndex, array.Length));
        }

        public bool Remove(View item)
        {
            if (item == null) return false;

            var index = Views.IndexOf(item);
            if (index >= 0)
            {
                item.Parent = null;
                item.Navigation = null;

                var removed = new List<View> { item };
                Views.Remove(item);

                OnRemoved(item);
                ChildrenRemoved?.Invoke(this, new LayoutEventArgs(index, 1, removed));
                return true;
            }

            return false;
        }

        protected virtual void OnRemoved(View view)
        {

        }

        public int Count => Views.Count;

        public bool IsReadOnly => false;

        public IEnumerator<View> GetEnumerator() => Views.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Views.GetEnumerator();

        public IReadOnlyList<View> GetChildren() => Views;

        public int IndexOf(View item) => Views.IndexOf(item);

        public void Insert(int index, View item)
        {
            if (item == null)
                return;

            Views.Insert(index, item);
            OnInsert(index, item);

            item.Parent = this;
            item.Navigation = Parent as NavigationView ?? Parent?.Navigation;
            ChildrenAdded?.Invoke(this, new LayoutEventArgs(index, 1));
        }

        protected virtual void OnInsert(int index, View item)
        {

        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < Views.Count)
            {
                var item = Views[index];
                item.Parent = null;
                item.Navigation = null;

                var removed = new List<View> { item };
                Views.RemoveAt(index);
                OnRemoved(item);

                ChildrenRemoved?.Invoke(this, new LayoutEventArgs(index, 1, removed));
            }
        }

        public View this[int index]
        {
            get => Views[index];
            set
            {
                var item = Views[index];
                item.Parent = null;
                item.Navigation = null;
                var removed = new List<View> { item };

                Views[index] = value;

                value.Parent = null;
                value.Navigation = null;

                ChildrenChanged?.Invoke(this, new LayoutEventArgs(index, 1, removed));
            }
        }

        protected override void OnParentChange(View parent)
        {
            base.OnParentChange(parent);
            foreach (var view in Views)
            {
                view.Parent = this;
                view.Navigation = parent as NavigationView ?? parent?.Navigation;
            }
        }
        internal override void ContextPropertyChanged(string property, object value)
        {
            base.ContextPropertyChanged(property, value);
            foreach (var view in Views)
            {
                view.ContextPropertyChanged(property, value);
            }
        }

        public override RectangleF Frame
        {
            get => base.Frame;
            set
            {
                base.Frame = value;
                RequestLayout();
            }
        }

        protected void OnFrameSet()
        {

        }
        protected override void Dispose(bool disposing)
        {
            foreach (var view in Views)
            {
                view.Dispose();
            }
            Views.Clear();
            base.Dispose(disposing);
        }
    }
}