using System;
using System.Threading.Tasks;
using HotUI.Graphics;

namespace HotUI.Services
{
    public interface IBitmapService
    {
        Task<Bitmap> LoadBitmapAsync(string source);

        Task<Bitmap> LoadBitmapFromUrlAsync(string source);

        Task<Bitmap> LoadBitmapFromFileAsync(string file);
    }
}