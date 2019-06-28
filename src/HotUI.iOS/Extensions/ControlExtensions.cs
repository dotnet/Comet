using System;
using System.Threading.Tasks;
using FFImageLoading;
using UIKit;

namespace HotUI.iOS {
	public static partial class ControlExtensions {
		public static Task<UIImage> LoadImage(this string source)
		{
			var isUrl = Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute);
			if (isUrl)
				return LoadImageAsync(source);
			return LoadFileAsync(source);
		}

		private static Task<UIImage> LoadImageAsync(string urlString)
		{
			return ImageService.Instance
				.LoadUrl(urlString)
				.AsUIImageAsync();
		}

		private static Task<UIImage> LoadFileAsync(string filePath)
		{
			return ImageService.Instance
				.LoadFile(filePath)
				.AsUIImageAsync();
		}

	}
}
