using System.Threading.Tasks;
using FFImageLoading;
using System.Maui.Graphics;
using System.Maui.Map.Graphics;
using System.Maui.Services;

namespace System.Maui.Mac.Services
{
	public class MacBitmapService : AbstractBitmapService
	{
		public override async Task<Bitmap> LoadBitmapFromUrlAsync(string url)
		{
			var image = await ImageService.Instance
				.LoadUrl(url)
				.AsNSImageAsync();

			return new NSImageBitmap(image);
		}

		public override async Task<Bitmap> LoadBitmapFromFileAsync(string file)
		{
			var image = await ImageService.Instance
				.LoadFile(file)
				.AsNSImageAsync();

			return new NSImageBitmap(image);
		}
	}
}
