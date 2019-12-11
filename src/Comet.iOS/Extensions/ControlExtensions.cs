using System;
using System.Threading.Tasks;
using FFImageLoading;
using UIKit;

namespace Comet.iOS
{
	public static partial class ControlExtensions
	{
		public static Task<UIImage> LoadImage(this string source)
		{
			var isUrl = Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute) && source.Contains("://");
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
