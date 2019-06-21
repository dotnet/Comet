using System;
using HotForms;
using Xamarin.Forms;

namespace HotUI.Forms {
	public class HotPageHandler : ContentPage , IFormsPage, IViewBuilderHandler {

		public Xamarin.Forms.Page Page => this;

		public void Remove (HotForms.View view)
		{
			Content = null;
		}

		public void SetView (HotForms.View view)
		{
			Content = view.ToForms ();

		}

		public void SetViewBuilder (ViewBuilder builder)
		{
			if (builder.View == null)
				builder.ReBuildView ();
			var hotPage = builder as HotPage;
			this.UpdateProperties (hotPage);
		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property, value);
		}
	}
}
