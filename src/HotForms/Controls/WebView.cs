using System;
using FControlType = Xamarin.Forms.WebView;
namespace HotForms {
	public class WebView : View<FControlType> {
		public string Html {
			get => (FormsControl.Source as Xamarin.Forms.HtmlWebViewSource)?.Html;
			set => FormsControl.Source = new Xamarin.Forms.HtmlWebViewSource { Html = value };
		}

		string source;
		public string Source {
			get => source;
			set {
				if (source == value)
					return;
				source = value;
				if (string.IsNullOrWhiteSpace (source)) {
					WebViewSource = null;
				}
				object s = source;
				var successs = Internal.BindingExpression.TryConvert (ref s, FControlType.SourceProperty, typeof (Xamarin.Forms.WebViewSource), true);
				if (successs)
					WebViewSource = (Xamarin.Forms.WebViewSource)s;
				else
					throw new Exception ("Source cannot be converted to an ImageSource");
			}
		}

		public Xamarin.Forms.WebViewSource WebViewSource {
			get => FormsControl.Source;
			set => FormsControl.Source = value;
		}
	}
}
