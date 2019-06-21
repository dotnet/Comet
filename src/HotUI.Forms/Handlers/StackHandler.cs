using System;
using HotUI;
using Xamarin.Forms;
using FStack = Xamarin.Forms.StackLayout;
using HStack = HotUI.Stack;
using HView = HotUI.View;
namespace HotUI.Forms {
	public class StackHandler : FStack, IViewHandler, IFormsView {

		public Xamarin.Forms.View View => this;

		public void Remove (HView view)
		{
			var s = view as HStack;
			if (s == null)
				return;
			s.ChildrenChanged -= Stack_ChildrenChanged;

		}
		HStack stack;
		public void SetView (HView view)
		{
			stack = view as HStack;
			if (stack == null)
				return;

			stack.ChildrenChanged += Stack_ChildrenChanged;
			this.UpdateProperties (stack);
			UpdateChildren (stack);
		}

		protected void UpdateChildren(Stack stack)
		{
			var children = stack.GetChildren ();
			var childrenCount = children.Count;
			var maxInt = Math.Max (children.Count, childrenCount);
			for (var i = 0; i < maxInt; i++) {
				if (i >= childrenCount) {
					Children.Remove (Children [i]);
					continue;
				}
				if (i >= Children.Count) {
					Children.Add (children [i].ToForms ());
				}
				var cView = children [i].ToForms ();
				if (Children [i] == cView)
					continue;
				Children [i] = cView;
			}
			
		}

		private void Stack_ChildrenChanged (object sender, EventArgs e)
		{
			UpdateChildren (stack);
		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateBaseProperty (property, value);
		}
	}
}
