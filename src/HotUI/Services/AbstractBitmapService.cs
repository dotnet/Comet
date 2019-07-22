using System;
using System.Threading.Tasks;
using HotUI.Graphics;

namespace HotUI.Services
{
    public abstract class AbstractBitmapService : IBitmapService
    {
        public Task<Bitmap> LoadBitmapAsync(string source)
        {
            var isUrl = Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute) && source.Contains("://");
            if (isUrl)
                return LoadBitmapFromUrlAsync(source);
            
            return LoadBitmapFromFileAsync(source);
        }

        public abstract Task<Bitmap> LoadBitmapFromUrlAsync(string url);

        public abstract Task<Bitmap> LoadBitmapFromFileAsync(string file);
    }
}