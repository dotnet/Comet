using System;
using System.Threading.Tasks;
using FFImageLoading;
using AppKit;

namespace HotUI.Mac {
	public static partial class ControlExtensions {
		public static Task<NSImage> LoadImage(this string source)
		{
			var isUrl = Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute) && source.Contains("://");
			if (isUrl)
				return LoadImageAsync(source);
			return LoadFileAsync(source);
		}

		private static Task<NSImage> LoadImageAsync(string urlString)
		{
			return ImageService.Instance
				.LoadUrl(urlString)
				.AsNSImageAsync();
		}

		private static Task<NSImage> LoadFileAsync(string filePath)
		{
			return ImageService.Instance
				.LoadFile(filePath)
				.AsNSImageAsync();
		}

	}
}
