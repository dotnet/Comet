using System;
using HotUI;
using fLayoutOptions = Xamarin.Forms.LayoutOptions;
namespace HotUI.Forms {
	public static class ControlsExtensions {

		public static void UpdateProperties (this Xamarin.Forms.ContentPage fView, HotPage view)
		{
			fView.Title = view.Title;
		}

		public static bool UpdateProperty (this Xamarin.Forms.ContentPage fView, string property, object value)
		{
			switch (property) {
			case nameof (HotPage.Title):
				fView.Title = (string)value;
				return true;
			}
			return false;
		}


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
			fView.Source = view.Source.ToImageSource ();
		}

		public static bool UpdateProperty (this Xamarin.Forms.Image fView, string property, object value)
		{
			switch (property) {
			case nameof (Image.Source):
				fView.Source = (value as string).ToImageSource ();
				return true;
			}
			return fView.UpdateBaseProperty (property, value);
		}

		public static Xamarin.Forms.ImageSource ToImageSource (this string source)
		{
			if (string.IsNullOrWhiteSpace (source)) {
				return null;
			}
			object s = source;
			var successs = HotUI.Internal.BindingExpression.TryConvert (ref s, Xamarin.Forms.Image.SourceProperty, typeof (Xamarin.Forms.ImageSource), true);
			if (successs)
				return (Xamarin.Forms.ImageSource)s;
			else
				throw new Exception ("Source cannot be converted to an ImageSource");
		}


		public static void UpdateProperties (this Xamarin.Forms.Entry fView, Entry view)
		{
			fView.UpdateBaseProperties (view);
			fView.Text = view.Text;
			fView.Placeholder = view.Placeholder;
		}

		public static bool UpdateProperty (this Xamarin.Forms.Entry fView, string property, object value)
		{
			switch (property) {
			case nameof (Entry.Text):
				fView.Text = (string)value;
				return true;
			case nameof (Entry.Placeholder):
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


		public static void UpdateProperties (this Xamarin.Forms.Label fView, Label label)
		{
			fView.UpdateBaseProperties (label);
			fView.Text = label.Text;
		}

		public static bool UpdateProperty (this Xamarin.Forms.Label fView, string property, object value)
		{
			switch (property) {
			case nameof (Label.Text):
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

		public static LayoutOptions Convert (this fLayoutOptions options)
		{
			if (options.Equals (fLayoutOptions.Center))
				return LayoutOptions.Center;
			if (options.Equals (fLayoutOptions.CenterAndExpand))
				return LayoutOptions.CenterAndExpand;
			if (options.Equals (fLayoutOptions.End))
				return LayoutOptions.End;
			if (options.Equals (fLayoutOptions.EndAndExpand))
				return LayoutOptions.EndAndExpand;
			if (options.Equals (fLayoutOptions.Fill))
				return LayoutOptions.Fill;
			if (options.Equals (fLayoutOptions.FillAndExpand))
				return LayoutOptions.FillAndExpand;
			if (options.Equals (fLayoutOptions.Start))
				return LayoutOptions.Start;
			if (options.Equals (fLayoutOptions.StartAndExpand))
				return LayoutOptions.StartAndExpand;
			throw new NotSupportedException ();
		}

		public static fLayoutOptions Convert (this LayoutOptions options)
		{
			if (options == LayoutOptions.Center)
				return fLayoutOptions.Center;
			if (options == LayoutOptions.CenterAndExpand)
				return fLayoutOptions.CenterAndExpand;
			if (options == LayoutOptions.End)
				return fLayoutOptions.End;
			if (options == LayoutOptions.EndAndExpand)
				return fLayoutOptions.EndAndExpand;
			if (options == LayoutOptions.Fill)
				return fLayoutOptions.Fill;
			if (options == LayoutOptions.FillAndExpand)
				return fLayoutOptions.FillAndExpand;
			if (options == LayoutOptions.Start)
				return fLayoutOptions.Start;
			if (options == LayoutOptions.StartAndExpand)
				return fLayoutOptions.StartAndExpand;
			throw new NotSupportedException ();
		}

		static bool IsSame (this fLayoutOptions options, fLayoutOptions otheroptions)
			=> options.Alignment == otheroptions.Alignment && options.Expands == otheroptions.Expands;
	}
}
