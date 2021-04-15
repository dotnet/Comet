using System.Threading.Tasks;
using FFImageLoading;
using Comet.Android.Graphics;
using Comet.Graphics;
using Comet.Services;

namespace Comet.Services
{
	public class BitmapService : AbstractBitmapService
	{
		public override async Task<Bitmap> LoadBitmapFromUrlAsync(string url)
		{
#if NET6_0
			return await Task.FromResult<Bitmap>(null);
#else
			var image = await ImageService.Instance
				.LoadUrl(url)
				.AsBitmapDrawableAsync();

			return new AndroidBitmap(image.Bitmap);
#endif
		}

		public override async Task<Bitmap> LoadBitmapFromFileAsync(string file)
		{
#if NET6_0
			return await Task.FromResult<Bitmap>(null);
#else
			try
			{
				var image = await ImageService.Instance
					.LoadFile(file)
					.AsBitmapDrawableAsync();

				return new AndroidBitmap(image.Bitmap);
			}
			catch
			{
				var image = await ImageService.Instance.LoadCompiledResource(file).AsBitmapDrawableAsync();
				return new AndroidBitmap(image.Bitmap);
			}
#endif
		}
	}
}
