using System;
using FControlType = Xamarin.Forms.Image;
namespace HotForms {
	public class Image : BaseControl<FControlType> {
		string source;
		public string Source {
			get => source;
			set {
				if (source == value)
					return;
				source = value;
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
			//get => FormsControl.Source = ;
			//set => FormsControl.Text = value;
		}
		public Xamarin.Forms.ImageSource ImageSource {
			get => FormsControl.Source;
			set => FormsControl.Source = value;
		}
	}
}
