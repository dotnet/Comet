using System;
using System.Threading.Tasks;
using Comet.Graphics;

namespace Comet.Services
{
	public interface IBitmapService
	{
		Task<Bitmap> LoadBitmapAsync(string source);

		Task<Bitmap> LoadBitmapFromUrlAsync(string source);

		Task<Bitmap> LoadBitmapFromFileAsync(string file);
	}
}
