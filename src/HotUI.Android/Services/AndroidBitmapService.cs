using System.Threading.Tasks;
using FFImageLoading;
using HotUI.Android.Graphics;
using HotUI.Graphics;
using HotUI.Services;

namespace HotUI.Android.Services
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
            var image = await  ImageService.Instance
                .LoadFile(file)
                .AsBitmapDrawableAsync();
            
            return new AndroidBitmap(image.Bitmap);
        }
    }
}