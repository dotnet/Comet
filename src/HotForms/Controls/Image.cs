using System;
using FControlType = Xamarin.Forms.Image;
namespace HotForms {
	public class Image : View<FControlType> {
		string source;
		public string Source {
			get => source;
			set => this.SetValue (State, ref source, value, setSource);
		}
		void setSource(object sourceString)
		{
			if (string.IsNullOrWhiteSpace (source)) {
				ImageSource = null;
			}
			object s = source;
			var successs = HotForms.Internal.BindingExpression.TryConvert (ref s, FControlType.SourceProperty, typeof (Xamarin.Forms.ImageSource), true);
			if (successs)
				ImageSource = (Xamarin.Forms.ImageSource)s;
			else
				throw new Exception ("Source cannot be converted to an ImageSource");
		}
		Xamarin.Forms.ImageSource imageSource;
		public Xamarin.Forms.ImageSource ImageSource {
			get => imageSource;
			set => this.SetValue (State, ref imageSource, value, setImageSource);
		}

		void setImageSource(object value)
		{
			if (IsControlCreated)
				this.FormsControl.Source = imageSource;
		}

		protected override void UnbindFormsView (object formsView)
		{

		}

		protected override void UpdateFormsView (object formsView)
		{
			var control = (FControlType)formsView;
			control.Source = imageSource;
		}
	}
}
