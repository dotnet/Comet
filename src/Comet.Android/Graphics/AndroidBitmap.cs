using System.Drawing;
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

        public override SizeF Size => _bitmap != null ? new SizeF(_bitmap.Width, _bitmap.Height) : SizeF.Empty;

        public override object NativeBitmap => _bitmap;

        protected override void DisposeNative()
        {
            _bitmap?.Dispose();
            _bitmap = null;
        }
    }
}
