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
