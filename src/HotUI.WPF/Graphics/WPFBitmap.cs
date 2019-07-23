using HotUI.Graphics;
using System.Windows.Media.Imaging;

namespace HotUI.WPF.Graphics
{
    public class WPFBitmap : Bitmap
    {
        private BitmapImage _image;

        public WPFBitmap(BitmapImage image)
        {
            _image = image;
        }

        public override SizeF Size => new SizeF(_image.PixelWidth, _image.PixelHeight);

        public override object NativeBitmap => _image;

        protected override void DisposeNative()
        {
            _image = null;
        }
    }
}
