using System;
using System.Threading.Tasks;
using Comet.Graphics;
namespace Comet.Services
{
	public class BitmapService : AbstractBitmapService
	{
		public override Task<Bitmap> LoadBitmapFromUrlAsync(string url) => throw new NotImplementedException();

		public override Task<Bitmap> LoadBitmapFromFileAsync(string file) => throw new NotImplementedException();
	}
}
