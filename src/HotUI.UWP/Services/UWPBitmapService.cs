using FFImageLoading;
using HotUI.Graphics;
using HotUI.Services;
using HotUI.UWP.Graphics;
using System.Threading.Tasks;

namespace HotUI.UWP.Services
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
