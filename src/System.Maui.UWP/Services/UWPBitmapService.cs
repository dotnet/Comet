using FFImageLoading;
using System.Maui.Graphics;
using System.Maui.Services;
using System.Maui.UWP.Graphics;
using System.Threading.Tasks;

namespace System.Maui.UWP.Services
{
	class UWPBitmapService : AbstractBitmapService
	{
		public override async Task<Bitmap> LoadBitmapFromUrlAsync(string url)
		{
			var image = await ImageService.Instance
				.LoadUrl(url)
				.AsWriteableBitmapAsync();

			return new UWPBitmap(image);
		}

		public override async Task<Bitmap> LoadBitmapFromFileAsync(string file)
		{
			var image = await ImageService.Instance
				.LoadFile(file)
				.AsWriteableBitmapAsync();

			return new UWPBitmap(image);
		}
	}
}
