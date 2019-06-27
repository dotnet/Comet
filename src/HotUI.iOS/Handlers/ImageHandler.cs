using System;
using UIKit;
using FFImageLoading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HotUI.iOS {
	public class ImageHandler : UIImageView, IUIView {
		public ImageHandler ()
		{
			this.Frame = new CoreGraphics.CGRect (0, 0, 56, 56);
		}

		public UIView View => this;

		public void Remove (View view)
		{
			//throw new NotImplementedException ();
		}

		public void SetView (View view)
		{
			var image = view as Image;
			this.UpdateProperties (image);
		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property,value);
		}
		string currentSource;
		public async void UpdateSource(string source)
		{
			if (source == currentSource)
				return;
			currentSource = source;
			try {
				var image = await source.LoadImage ();
				if (source == currentSource)
					this.Image = image;
			} catch(Exception e) {
				Debug.WriteLine (e);
			}
		}
	}

	public static partial class ControlExtensions {

		public static void UpdateProperties (this ImageHandler view, Image hView)
		{
			view.UpdateSource (hView.Source);
			view.UpdateBaseProperties (hView);
		}

		public static bool UpdateProperty (this ImageHandler view, string property, object value)
		{
			switch (property) {
			case nameof (Image.Source):
				view.UpdateSource ((string)value);
				return true;
			}
			return view.UpdateBaseProperty (property, value);
		}

		public static Task<UIImage> LoadImage(this string source)
		{
			var isUrl = Uri.IsWellFormedUriString (source, UriKind.RelativeOrAbsolute);
			if (isUrl)
				return LoadImageAsync (source);
			return LoadFileAsync (source);
		}

		private static Task<UIImage> LoadImageAsync (string urlString)
		{
			return ImageService.Instance
				.LoadUrl (urlString)
				.AsUIImageAsync ();
		}

		private static Task<UIImage> LoadFileAsync (string filePath)
		{
			return ImageService.Instance
				.LoadFile (filePath)
				.AsUIImageAsync ();
		}

	}
}
