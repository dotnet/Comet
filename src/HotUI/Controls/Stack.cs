using System;
using System.Collections;
using System.Collections.Generic;
namespace HotForms {


	public class Stack : View, IEnumerable, IEnumerable<View>, IContainerView {

		public IEnumerator<View> GetEnumerator () => views.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator () => views.GetEnumerator ();

		List<View> views = new List<View> ();
		public void Add (View view)
		{
			if (view == null)
				return;
			views.Add (view);
			ChildrenChanged?.Invoke (this,EventArgs.Empty);
		}

		public event EventHandler ChildrenChanged;
		public IReadOnlyList<View> GetChildren () => views;
	}
}
