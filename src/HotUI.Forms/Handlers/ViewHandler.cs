using System;
using Xamarin.Forms;
using FView = Xamarin.Forms.ContentView;
using HView = HotUI.View;

namespace HotUI.Forms {
	public class ViewHandler : FView, IFormsView {
		public Xamarin.Forms.View View => this;

		public void Remove (HView view)
		{

		}

		public void SetView (HView view)
		{
			Content = view.ToForms ();
			this.UpdateBaseProperties (view);
		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property, value);
		}
	}
}
