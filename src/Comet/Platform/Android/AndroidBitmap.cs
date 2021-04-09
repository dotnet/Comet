using Microsoft.Maui.Graphics;
using ABitmap = Android.Graphics.Bitmap;

namespace Comet.Android.Graphics
{
	public class AndroidBitmap : Comet.Graphics.Bitmap
	{
		private ABitmap _bitmap;

		public AndroidBitmap(ABitmap image)
		{
			_bitmap = image;
		}

		public override Size Size => _bitmap != null ? new Size(_bitmap.Width, _bitmap.Height) : Size.Zero;

		public override object NativeBitmap => _bitmap;

		protected override void DisposeNative()
		{
			_bitmap?.Dispose();
			_bitmap = null;
		}
	}
}
