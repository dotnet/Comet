using System;
using HotForms;
using Xamarin.Forms;
using FWebView = Xamarin.Forms.WebView;
using HWebView = HotForms.WebView;
using HView = HotForms.View;
namespace HotUI.Forms {
	public class WebViewHandler : FWebView, IViewHandler, IFormsView {
		public WebViewHandler ()
		{
		}

		public Xamarin.Forms.View View => this;

		public void Remove (HView view)
		{

		}

		public void SetView (HView view)
		{
			var webView = view as HWebView;
			if (webView == null) {
				return;
			}
			this.UpdateProperties (webView);
			throw new NotImplementedException ();
		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateBaseProperty (property, value);
		}
	}
}
