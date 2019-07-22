using HotUI.Graphics;
using Windows.UI.Xaml.Media.Imaging;

namespace HotUI.UWP.Graphics
{
    public class UWPBitmap : Bitmap
    {
        private WriteableBitmap _image;

        public UWPBitmap(WriteableBitmap image)
        {
            _image = image;
        }

        public override SizeF Size => _image != null ? new SizeF(_image.PixelWidth, _image.PixelHeight) : SizeF.Zero;

        public override object NativeBitmap => _image;

        protected override void DisposeNative()
        {
            _image = null;
        }
    }
}
