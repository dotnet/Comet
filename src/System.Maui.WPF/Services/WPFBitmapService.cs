using System.Maui.Graphics;
using System.Maui.Services;
using System.Maui.WPF.Graphics;
using System;
using System.Net.Cache;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace System.Maui.WPF.Services
{
	class WPFBitmapService : AbstractBitmapService
	{
		public override Task<Bitmap> LoadBitmapFromUrlAsync(string url)
			=> LoadBitmapAsync(new Uri(url, UriKind.Absolute));

		public override Task<Bitmap> LoadBitmapFromFileAsync(string file)
		{
			var fullPath = System.IO.Path.GetFullPath(file);
			return LoadBitmapAsync(new Uri(fullPath));
		}

		private static Task<Bitmap> LoadBitmapAsync(Uri location)
		{
			var bitmap = new BitmapImage(location, new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable));
			var wrapper = new WPFBitmap(bitmap);
			if (!bitmap.IsDownloading)
			{
				return Task.FromResult<Bitmap>(wrapper);
			}

			TaskCompletionSource<Bitmap> completionSource = new TaskCompletionSource<Bitmap>();
			bitmap.DownloadCompleted += (_, __) => completionSource.SetResult(wrapper);
			bitmap.DownloadFailed += (_, args) => completionSource.SetException(args.ErrorException);
			bitmap.DecodeFailed += (_, args) => completionSource.SetException(args.ErrorException);
			return completionSource.Task;
		}

	}
}
