using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public abstract class ImageSource : IImageSource
	{
		public abstract bool IsEmpty { get; }

		public static implicit operator ImageSource(string value)
		{
			var isUrl = Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute) && value.Contains("://");
			if (isUrl)
				return new UriImageSource { Uri = new(value) };
			return new FileImageSource { File = value };
		}
	}

	public class FileImageSource : ImageSource , IFileImageSource
	{
		public string File { get; set; }

		public override bool IsEmpty => string.IsNullOrWhiteSpace(File);

	}

	public class UriImageSource : ImageSource, IUriImageSource
	{

		public override bool IsEmpty => Uri == null;

		public Uri Uri { get; set; }

		public static TimeSpan DefaultCacheValidity = TimeSpan.MaxValue;
		private TimeSpan? cacheValidity;
		public TimeSpan CacheValidity { get => cacheValidity ?? DefaultCacheValidity; set => cacheValidity = value; }

		public bool CachingEnabled { get; set; } = true;
	}

	public class FontImageSource : ImageSource, IFontImageSource
	{
		public override bool IsEmpty => throw new NotImplementedException();

		public Color Color { get; set; } = Colors.Black;

		public Font Font { get; set; } = Font.Default;

		public string Glyph { get; set; }
	}
}
