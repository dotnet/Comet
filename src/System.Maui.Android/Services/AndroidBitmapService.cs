using System.Threading.Tasks;
using FFImageLoading;
using System.Maui.Android.Graphics;
using System.Maui.Graphics;
using System.Maui.Services;

namespace System.Maui.Android.Services
{
	public class AndroidBitmapService : AbstractBitmapService
	{
		public override async Task<Bitmap> LoadBitmapFromUrlAsync(string url)
		{
			var image = await ImageService.Instance
				.LoadUrl(url)
				.AsBitmapDrawableAsync();

			return new AndroidBitmap(image.Bitmap);
		}

		public override async Task<Bitmap> LoadBitmapFromFileAsync(string file)
		{
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
		}
	}
}
