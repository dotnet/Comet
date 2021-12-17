using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;

namespace Comet
{
	public class ContainerView : View, IList<View>, IContainerView, IContainer
	{
		readonly protected List<View> Views = new List<View>();

		public void Add(IEnumerable<View> views)
		{
			foreach (var v in views)
				Add(v);
		}
		public void Add(IEnumerable<IView> views)
		{
			foreach (var v in views)
				Add(v);
		}
		public void Add(IView iView)
		{
			//TODO: Add wrapper
			if (iView is View v)
				Add(v);
			throw new NotImplementedException();
		}
		public void Add(View view)
		{
			if (view == null)
				return;
			view.Parent = this;
			view.Navigation = Parent as NavigationView ?? Parent?.Navigation;
			Views.Add(view);
			OnAdded(view);
			TypeHashCode = null;
		}

		protected virtual void OnAdded(View view)
		{
			//ViewHandler?.UpdateValue(nameof(IContainer.Children));
		}

		public void Clear()
		{
			TypeHashCode = null;
			var count = Views.Count;
			if (count > 0)
			{
				var removed = new List<View>(Views);
				Views.Clear();
				OnClear(removed);
			}
		}

		protected virtual void OnClear(List<View> views)
		{
			//ViewHandler?.UpdateValue(nameof(IContainer.Children));
		}

		public bool Contains(View item) => Views.Contains(item);

		public void CopyTo(View[] array, int arrayIndex)
		{
			Views.CopyTo(array, arrayIndex);
		}

		public bool Remove(IView iView)
		{

			//TODO: Add wrapper
			if (iView is View v)
				return Remove(v);
			throw new NotImplementedException();
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
				TypeHashCode = null;
				//ChildrenRemoved?.Invoke(this, new LayoutEventArgs(index, 1, removed));
				return true;
			}

			return false;
		}

		protected virtual void OnRemoved(View view)
		{
			//ViewHandler?.UpdateValue(nameof(IContainer.Children));
		}

		public int Count => Views.Count;

		public bool IsReadOnly => false;

		IView IList<IView>.this[int index] { get => Views[index]; set => Views[index] = value as View; }

		public IEnumerator<View> GetEnumerator() => Views.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => Views.GetEnumerator();

		public int IndexOf(View item) => Views.IndexOf(item);

		public void Insert(int index, View item)
		{
			if (item == null)
				return;

			Views.Insert(index, item);
			OnInsert(index, item);

			item.Parent = this;
			item.Navigation = Parent as NavigationView ?? Parent?.Navigation;
			TypeHashCode = null;
		}

		protected virtual void OnInsert(int index, View item)
		{
			//ViewHandler?.UpdateValue(nameof(IContainer.Children));
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
				TypeHashCode = null;
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

//				ChildrenChanged?.Invoke(this, new LayoutEventArgs(index, 1, removed));
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
		internal override void ContextPropertyChanged(string property, object value, bool cascades)
		{
			base.ContextPropertyChanged(property, value, cascades);
			if (!cascades)
				return;
			foreach (var view in Views)
			{
				view.ContextPropertyChanged(property, value, cascades);
			}
		}

		protected override void Dispose(bool disposing)
		{
			foreach (var view in Views)
			{
				view.Dispose();
			}
			Views?.Clear();
			base.Dispose(disposing);
		}

		public override void ViewDidAppear()
		{
			Views?.ForEach(v => v.ViewDidAppear());
			base.ViewDidAppear();
		}

		public override void ViewDidDisappear()
		{
			Views?.ForEach(v => v.ViewDidDisappear());
			base.ViewDidDisappear();
		}

		public override void PauseAnimations()
		{
			Views?.ForEach(v => v.PauseAnimations());
			base.PauseAnimations();
		}
		public override void ResumeAnimations()
		{
			Views?.ForEach(v => v.ResumeAnimations());
			base.ResumeAnimations();
		}

		public override int GetContentTypeHashCode() => TypeHashCode ??= GetChidrensTypeHasCode();

		int GetChidrensTypeHasCode()
		{
			int hashCode = base.GetContentTypeHashCode();
			foreach (var v in Views)
				hashCode = (hashCode, v.GetType().GetHashCode()).GetHashCode();
			return hashCode;
		}

		int IList<IView>.IndexOf(IView item) => Views.IndexOf(item);
		void IList<IView>.Insert(int index, IView item) => this.Insert(index, (View)item);
		bool ICollection<IView>.Contains(IView item) => Views.Contains(item);
		void ICollection<IView>.CopyTo(IView[] array, int arrayIndex) => Views.OfType<IView>().ToArray().CopyTo(array, arrayIndex);
		IEnumerator<IView> IEnumerable<IView>.GetEnumerator() => Views.OfType<IView>().GetEnumerator();
		IReadOnlyList<View> IContainerView.GetChildren() => this.Views;
	}
}
