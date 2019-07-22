using HotUI.Graphics;

namespace HotUI.WPF.Graphics
{
    public class WPFBitmap : Bitmap
    {
        private object _image;

        public WPFBitmap(object image)
        {
            _image = image;
        }

        //public override SizeF Size => _image != null ? new SizeF(_image.PixelWidth, _image.PixelHeight) : SizeF.Zero;
        public override SizeF Size => SizeF.Zero;

        public override object NativeBitmap => _image;

        protected override void DisposeNative()
        {
            _image = null;
        }
    }
}
