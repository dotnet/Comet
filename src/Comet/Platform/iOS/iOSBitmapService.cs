using System.Threading.Tasks;
using FFImageLoading;
using Comet.Graphics;
using Comet.iOS.Graphics;
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
				.AsUIImageAsync();

			return new UIImageBitmap(image);
#endif
		}

		public override async Task<Bitmap> LoadBitmapFromFileAsync(string file)
		{
#if NET6_0
			return await Task.FromResult<Bitmap>(null);
#else
			var image = await ImageService.Instance
				.LoadFile(file)
				.AsUIImageAsync();

			return new UIImageBitmap(image);
#endif
		}
	}
}
