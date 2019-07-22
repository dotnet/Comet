using System;
using HotUI;
using HotUI.Graphics;
using fLayoutOptions = Xamarin.Forms.LayoutOptions;
namespace HotUI.Forms {
	public static class ControlsExtensions {

		public static void UpdateProperties (this Xamarin.Forms.WebView fView, WebView view)
		{
			fView.UpdateBaseProperties (view);
			if (string.IsNullOrWhiteSpace (view.Html))
				fView.Source = new Xamarin.Forms.HtmlWebViewSource { Html = view.Html };
			fView.Source = view.Source.ToWebViewSource ();
		}

		public static bool UpdateProperty (this Xamarin.Forms.WebView fView, string property, object value)
		{
			switch (property) {
			case nameof (WebView.Html):
				fView.Source = value == null ? null : new Xamarin.Forms.HtmlWebViewSource { Html = (string)value };
				return true;
			case nameof (WebView.Source):
				fView.Source = (value as string).ToWebViewSource ();
				return true;
			}
			return fView.UpdateBaseProperty (property, value);
		}

		public static Xamarin.Forms.WebViewSource ToWebViewSource (this string source)
		{

			if (string.IsNullOrWhiteSpace (source)) {
				return null;
			}
			object s = source;
			var successs = HotUI.Internal.BindingExpression.TryConvert (ref s, Xamarin.Forms.WebView.SourceProperty, typeof (Xamarin.Forms.WebViewSource), true);
			if (successs)
				return (Xamarin.Forms.WebViewSource)s;
			else
				throw new Exception ("Source cannot be converted to an ImageSource");
		}


		public static void UpdateProperties (this Xamarin.Forms.Image fView, Image view)
		{
			fView.UpdateBaseProperties (view);
			fView.Source = view.Bitmap.ToImageSource ();
		}

		public static bool UpdateProperty (this Xamarin.Forms.Image fView, string property, object value)
		{
			switch (property) {
			case nameof (Image.Bitmap):
				fView.Source = (value as Bitmap).ToImageSource ();
				return true;
			}
			return fView.UpdateBaseProperty (property, value);
		}

		public static Xamarin.Forms.ImageSource ToImageSource (this Bitmap bitmap)
		{
			object s = bitmap;
			var success = HotUI.Internal.BindingExpression.TryConvert (ref s, Xamarin.Forms.Image.SourceProperty, typeof (Xamarin.Forms.ImageSource), true);
			if (success)
				return (Xamarin.Forms.ImageSource)s;
			else
				throw new Exception ("Source cannot be converted to an ImageSource");
		}


		public static void UpdateProperties (this Xamarin.Forms.Entry fView, TextField view)
		{
			fView.UpdateBaseProperties (view);
			fView.Text = view.Text;
			fView.Placeholder = view.Placeholder;
		}

		public static bool UpdateProperty (this Xamarin.Forms.Entry fView, string property, object value)
		{
			switch (property) {
			case nameof (TextField.Text):
				fView.Text = (string)value;
				return true;
			case nameof (TextField.Placeholder):
				fView.Placeholder = (string)value;
				return true;
			}
			return fView.UpdateBaseProperty (property, value);
		}

		public static void UpdateProperties (this Xamarin.Forms.Button fView, Button view)
		{
			fView.UpdateBaseProperties (view);
			fView.Text = view.Text;
		}

		public static bool UpdateProperty (this Xamarin.Forms.Button fView, string property, object value)
		{
			switch (property) {
			case nameof (Button.Text):
				fView.Text = (string)value;
				return true;
			}
			return fView.UpdateBaseProperty (property, value);
		}


		public static void UpdateProperties (this Xamarin.Forms.Label fView, Text text)
		{
			fView.UpdateBaseProperties (text);
			fView.Text = text.Value;
		}

		public static bool UpdateProperty (this Xamarin.Forms.Label fView, string property, object value)
		{
			switch (property) {
			case nameof (Text.Value):
				fView.Text = (string)value;
				return true;
			}
			return fView.UpdateBaseProperty (property, value);
		}



		public static void UpdateBaseProperties (this Xamarin.Forms.View fView, View view)
		{
			fView.UpdateProperties (view);
		}

		public static void UpdateProperties (this Xamarin.Forms.View fView, View view)
		{

		}

		public static bool UpdateBaseProperty (this Xamarin.Forms.View fView, string property, object value)
		{
			return fView.UpdateProperty (property, value);
		}

		public static bool UpdateProperty (this Xamarin.Forms.View fView, string property, object value)
		{

			//TODO: Fix this
			return false;
		}

		static bool IsSame (this fLayoutOptions options, fLayoutOptions otheroptions)
			=> options.Alignment == otheroptions.Alignment && options.Expands == otheroptions.Expands;
	}
}
