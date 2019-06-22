using System;
using System.Collections.Generic;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers {
	public class StackHandler : NSStackView, IViewHandler, INSView {

		public StackHandler ()
		{
			this.Orientation = NSUserInterfaceLayoutOrientation.Vertical;
			this.Alignment = NSLayoutAttribute.Left & NSLayoutAttribute.Right;
			this.Spacing = 0;
			this.Distribution = NSStackViewDistribution.Fill;
			
			this.Layer.BackgroundColor = NSColor.Green.CGColor;
			this.TranslatesAutoresizingMaskIntoConstraints = false;
		}

		public NSView View => this;
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
		List<NSView> views = new List<NSView> ();
		protected void UpdateChildren (Stack stack)
		{
			views.ForEach (x => {
				RemoveArrangedSubview (x);
				x.RemoveFromSuperview ();
			});
			views.Clear ();
			foreach (var child in stack.GetChildren ()) {
				var cview = child.ToView ();
				views.Add (cview);
				//cview.ContentMode = NSViewContentMode.Top;
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
