using System.Threading.Tasks;
using FFImageLoading;
using HotUI.Graphics;
using HotUI.Map.Graphics;
using HotUI.Services;

namespace HotUI.iOS.Services
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
            var image = await  ImageService.Instance
                .LoadFile(file)
                .AsNSImageAsync();
            
            return new NSImageBitmap(image);
        }
    }
}