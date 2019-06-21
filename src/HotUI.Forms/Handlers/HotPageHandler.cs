using System;
using HotUI;
using Xamarin.Forms;

namespace HotUI.Forms {
	public class HotPageHandler : ContentPage, IFormsPage, IViewBuilderHandler {

		public Xamarin.Forms.Page Page => this;

		public void Remove (HotUI.View view)
		{
			Content = null;
		}

		public void SetView (HotUI.View view)
		{
			var newView = view.ToForms ();
			if (newView == Content)
				return;
			Content = newView;

		}

		HotPage hotPage;
		public void SetViewBuilder (ViewBuilder builder)
		{
			hotPage = builder as HotPage;
			if (builder.View == null)
				builder.ReBuildView ();
			this.UpdateProperties (hotPage);
		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property, value);
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			hotPage?.OnAppearing ();
		}
		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			hotPage?.OnDisppearing ();
		}
	}
}
