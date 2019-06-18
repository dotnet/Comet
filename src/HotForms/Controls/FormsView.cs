using System;
namespace HotForms {

	public class FormsView : View {

		public Type FormsViewType { get; private set; }
		public FormsView (Xamarin.Forms.View view)
		{
			FormsViewType = view.GetType ();
			FormsView = view;
			view.BindingContext = this;
		}


		protected override object CreateFormsView ()
		{
			//This should never be called!
			throw new NotImplementedException ();
		}

		protected override void UnbindFormsView (object formsView)
		{
		}

		protected override void UpdateFormsView (object formsView)
		{
		}
	}
}
