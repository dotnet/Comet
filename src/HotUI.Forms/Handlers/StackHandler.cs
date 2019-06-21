using System;
using HotForms;
using Xamarin.Forms;
using FStack = Xamarin.Forms.StackLayout;
using HStack = HotForms.Stack;
using HView = HotForms.View;
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
			Children.Clear ();
			foreach (var v in stack.GetChildren ()) {
				Children.Add (v.ToForms ());
			}
		}

		private void Stack_ChildrenChanged (object sender, EventArgs e)
		{
			Children.Clear ();
			foreach (var v in stack.GetChildren ()) {
				Children.Add (v.ToForms ());
			}
		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateBaseProperty (property, value);
		}
	}
}
