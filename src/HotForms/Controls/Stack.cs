using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;
using FControlType = Xamarin.Forms.StackLayout;
namespace HotForms {


	public class Stack : View<FControlType>, IEnumerable, IEnumerable<Xamarin.Forms.View> {

		public IEnumerator<Xamarin.Forms.View> GetEnumerator () => FormsControl.Children.GetEnumerator ();
		IEnumerator IEnumerable.GetEnumerator () => FormsControl.Children.GetEnumerator ();
		public void Add (Xamarin.Forms.View view)
		{
			if (view == null)
				return;
			FormsControl.Children.Add (view);
		}

		public Stack AsHorizontal()
		{
			FormsControl.Orientation = StackOrientation.Horizontal;
			return this;
		}

		public Stack AsVertical ()
		{
			FormsControl.Orientation = StackOrientation.Vertical;
			return this;
		}
	}
}
