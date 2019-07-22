using ABitmap = Android.Graphics.Bitmap;

namespace HotUI.Android.Graphics
{
    public class AndroidBitmap : HotUI.Graphics.Bitmap
    {
        private ABitmap _bitmap;

        public AndroidBitmap(ABitmap image)
        {
            _bitmap = image;
        }

        public override SizeF Size => _bitmap != null ? new SizeF(_bitmap.Width, _bitmap.Height) : SizeF.Zero;

        public override object NativeBitmap => _bitmap;

        protected override void DisposeNative()
        {
            _bitmap?.Dispose();
            _bitmap = null;
        }
    }
}
