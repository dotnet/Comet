using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers {
	public class ImageHandler : NSImageView, INSView {
		public ImageHandler ()
		{
		}

		public NSView View => throw new NotImplementedException ();

		public void Remove (View view)
		{
			throw new NotImplementedException ();
		}

		public void SetView (View view)
		{
			throw new NotImplementedException ();
		}

		public void UpdateValue (string property, object value)
		{
			throw new NotImplementedException ();
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

		public static Task<NSImage> LoadImage(this string source)
		{
			var isUrl = Uri.IsWellFormedUriString (source, UriKind.RelativeOrAbsolute);
			if (isUrl)
				return LoadImageAsync (source);
			return LoadFileAsync (source);
		}

		private static Task<NSImage> LoadImageAsync (string urlString)
		{
			throw new NotImplementedException();
			/*
			return ImageService.Instance
				.LoadUrl (urlString)
				.AsUIImageAsync ();*/
		}

		private static Task<NSImage> LoadFileAsync (string filePath)
		{
			throw new NotImplementedException();
			
			/*
			return ImageService.Instance
				.LoadFile (filePath)
				.AsUIImageAsync ();*/
		}

	}
}
