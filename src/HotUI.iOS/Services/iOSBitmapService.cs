using System.Threading.Tasks;
using FFImageLoading;
using HotUI.Graphics;
using HotUI.iOS.Graphics;
using HotUI.Services;

namespace HotUI.iOS.Services
{
    public class iOSBitmapService : AbstractBitmapService
    {
        public override async Task<Bitmap> LoadBitmapFromUrlAsync(string url)
        {
            var image = await ImageService.Instance
                .LoadUrl(url)
                .AsUIImageAsync();
            
            return new UIImageBitmap(image);
        }

        public override async Task<Bitmap> LoadBitmapFromFileAsync(string file)
        {
            var image = await  ImageService.Instance
                .LoadFile(file)
                .AsUIImageAsync();
            
            return new UIImageBitmap(image);
        }
    }
}