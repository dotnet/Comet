using System;
using System.Collections;
using System.Collections.Generic;
namespace HotUI {


	public class Stack : View, IEnumerable, IEnumerable<View>, IContainerView {

		public IEnumerator<View> GetEnumerator () => views.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator () => views.GetEnumerator ();

		List<View> views = new List<View> ();
		public void Add (View view)
		{
			if (view == null)
				return;
			view.Parent = this;
			view.Navigation = this.Navigation;
			views.Add (view);
			ChildrenChanged?.Invoke (this,EventArgs.Empty);
		}

		public event EventHandler ChildrenChanged;
		public IReadOnlyList<View> GetChildren () => views;
		protected override void OnParentChange (View parent)
		{
			base.OnParentChange (parent);
			foreach (var view in views) {
				view.Parent = this;
			}
		}
	}
}
