using System;
using System.Collections;
using System.Collections.Generic;

namespace HotUI {
	public class ContentView : View, IEnumerable {
		IEnumerator IEnumerable.GetEnumerator () => new [] { Content }.GetEnumerator ();
		public View Content { get; set; }
		public virtual void Add (View view)
		{
			if (view == null)
				return;
			view.Parent = this;
			view.Navigation = this.Parent?.Navigation;
			Content = view;
		}
		protected override void OnParentChange (View parent)
		{
			base.OnParentChange (parent);
			if (Content != null) {
				Content.Parent = this;
			}
		}

	}
}
