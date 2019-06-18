using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;
using FControlType = Xamarin.Forms.StackLayout;
namespace HotForms {


	public class Stack : View<FControlType>, IEnumerable, IEnumerable<View>, IContainerView {

		public IEnumerator<View> GetEnumerator () => views.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator () => views.GetEnumerator ();

		List<View> views = new List<View> ();
		public void Add (View view)
		{
			if (view == null)
				return;
			views.Add (view);
			if(this.IsControlCreated)
				FormsControl.Children.Add (view);
		}

		public void Add (Xamarin.Forms.View view)
		{
			if (view == null)
				return;
			Add (new FormsView (view));
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

		public IReadOnlyList<View> GetChildren () => views;

		

		protected override void UnbindFormsView (object formsView)
		{
			var control = (FControlType)formsView;
			control.Children.Clear ();
		}

		protected override void UpdateFormsView (object formsView)
		{
			var control = (FControlType)formsView;
			foreach (var v in views) {
				
				control.Children.Add (v);
			}
		}
	}
}
