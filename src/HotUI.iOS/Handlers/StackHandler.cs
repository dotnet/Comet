using System;
using System.Collections.Generic;
using UIKit;
namespace HotUI.iOS {
	public class StackHandler : UIStackView, IUIView {

		public StackHandler ()
		{
			this.Axis = UILayoutConstraintAxis.Vertical;
			this.Alignment = UIStackViewAlignment.Fill;
			this.Spacing = 0;
			this.Distribution = UIStackViewDistribution.Fill;
			
			this.BackgroundColor = UIColor.Green;
			this.TranslatesAutoresizingMaskIntoConstraints = false;
		}

		public UIView View => this;
		public void Remove (View view)
		{

		}
		Stack stack;
		public void SetView (View view)
		{
			stack = view as Stack;
			UpdateChildren (stack);
			stack.ChildrenChanged += Stack_ChildrenChanged;
		}

		private void Stack_ChildrenChanged (object sender, EventArgs e)
		{
			UpdateChildren (stack);
		}

		public void UpdateValue (string property, object value)
		{

		}
		List<UIView> views = new List<UIView> ();
		protected void UpdateChildren (Stack stack)
		{
			var children = stack.GetChildren ();
			if (views.Count == children.Count) {
				bool areSame = false;
				for (var i = 0; i < views.Count; i++) {
					var v = views [i];
					var c = children [i].ToView ();
					areSame = c == v;
					if (!areSame) {
						break;
					}
				}
				if (areSame)
					return;
			}

			foreach (var v in views) {
				RemoveArrangedSubview (v);
				v.RemoveFromSuperview ();
			}
			views.Clear ();
			foreach (var child in children) {
				var cview = child.ToView ();
				views.Add (cview);
				//cview.ContentMode = UIViewContentMode.Top;
				AddSubview (cview);
				AddArrangedSubview (cview);

			}


			//var children = stack.GetChildren ();
			//var childrenCount = children.Count;
			//var maxInt = Math.Max (Subviews.Length, childrenCount);
			//for (var i = 0; i < maxInt; i++) {
			//	if (i >= childrenCount) {
			//		Subviews [i].RemoveFromSuperview ();
			//		continue;
			//	}
			//	if (i >= Subviews.Length) {
			//		this.Add (children [i].ToView ());
			//	}
			//	var cView = children [i].ToView ();
			//	if (Subviews [i] == cView)
			//		continue;

			//	Subviews [i].RemoveFromSuperview();
			//	InsertSubview (cView, i);
			//}

		}
	}
}
