//using FFImageLoading;
using HotUI.Graphics;
using HotUI.Services;
using HotUI.WPF.Graphics;
using System.Threading.Tasks;

namespace HotUI.WPF.Services
{
    class UWPBitmapService : AbstractBitmapService
    {
        public override async Task<Bitmap> LoadBitmapFromUrlAsync(string url)
        {
            return new WPFBitmap(null);
        }

        public override async Task<Bitmap> LoadBitmapFromFileAsync(string file)
        {
            return new WPFBitmap(null);
        }
    }
}
