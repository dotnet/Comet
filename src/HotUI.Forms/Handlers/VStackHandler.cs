using System;
using System.Diagnostics;
using HotUI;
using Xamarin.Forms;
using FStack = Xamarin.Forms.StackLayout;
using HStack = HotUI.VStack;
using HView = HotUI.View;

namespace HotUI.Forms
{
	public class VStackHandler : FStack, IFormsView
    {
		public Xamarin.Forms.View View => this;

		public void Remove (HView view)
		{
			var s = view as VStack;
			if (s == null)
				return;
			s.ChildrenChanged -= Stack_ChildrenChanged;
		}

		VStack stack;
		public void SetView (HView view)
		{
			stack = view as VStack;
			if (stack == null)
				return;

			stack.ChildrenChanged += Stack_ChildrenChanged;
			this.UpdateProperties (stack);
			UpdateChildren (stack);
		}

		protected void UpdateChildren(VStack stack)
		{
            //Clearing seems to be faster. Also, it flashes on android no matter what.
            Children.Clear ();
			foreach (var child in stack.GetChildren ())
            {
                var nativeView = child.ToForms();
                if (nativeView != null)
                    Children.Add (nativeView);
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
