using System;
using FControlType = Xamarin.Forms.WebView;
namespace HotForms {
	public class WebView : View<FControlType> {
		string html;
		public string Html {
			get => html;
			set => this.SetValue (State, ref html, value, setHtml);
		}
		void setHtml (object sourceString)
		{
			var value = (string)sourceString;
			WebViewSource = new Xamarin.Forms.HtmlWebViewSource { Html = value };
		}


		string source;
		public string Source {
			get => source;
			set => this.SetValue (State, ref source, value, setSource);
		}

		void setSource (object sourceString)
		{
			var value = (string)sourceString;
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
		

		Xamarin.Forms.WebViewSource webViewSource;
		public Xamarin.Forms.WebViewSource WebViewSource {
			get => webViewSource;
			set => this.SetValue (State, ref webViewSource, value, setWebViewSource);
		}

		void setWebViewSource (object value)
		{
			if (IsControlCreated)
				this.FormsControl.Source = webViewSource;
		}

		protected override void UnbindFormsView (object formsView)
		{

		}

		protected override void UpdateFormsView (object formsView)
		{
			var control = (FControlType)formsView;
			control.Source = webViewSource;
		}
	}
}
