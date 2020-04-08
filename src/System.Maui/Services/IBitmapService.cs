using System;
using System.Threading.Tasks;
using System.Maui.Graphics;

namespace System.Maui.Services
{
	public interface IBitmapService
	{
		Task<Bitmap> LoadBitmapAsync(string source);

		Task<Bitmap> LoadBitmapFromUrlAsync(string source);

		Task<Bitmap> LoadBitmapFromFileAsync(string file);
	}
}
