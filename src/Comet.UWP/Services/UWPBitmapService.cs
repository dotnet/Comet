using FFImageLoading;
using Comet.Graphics;
using Comet.Services;
using Comet.UWP.Graphics;
using System.Threading.Tasks;

namespace Comet.UWP.Services
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
