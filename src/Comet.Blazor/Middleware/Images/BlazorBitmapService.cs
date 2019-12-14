using Comet.Graphics;
using Comet.Services;
using System.Threading.Tasks;

namespace Comet.Blazor.Middleware.Images
{
	internal class BlazorBitmapService : IBitmapService
	{
		private readonly BitmapRepository _repo;

		public BlazorBitmapService(BitmapRepository repo)
		{
			_repo = repo;
		}

		public Task<Bitmap> LoadBitmapAsync(string source)
		{
			var bitmap = new BlazorBitmap(source, _repo.Remove);

			_repo.Add(bitmap);

			return Task.FromResult<Bitmap>(bitmap);
		}

		public Task<Bitmap> LoadBitmapFromFileAsync(string file) => LoadBitmapAsync(file);

		public Task<Bitmap> LoadBitmapFromUrlAsync(string source) => LoadBitmapAsync(source);
	}
}
