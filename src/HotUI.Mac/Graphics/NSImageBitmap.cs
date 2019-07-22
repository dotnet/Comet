using HotUI.Graphics;
using AppKit;
using HotUI.Mac;

namespace HotUI.Map.Graphics
{
    public class NSImageBitmap : Bitmap
    {
        private NSImage _image;

        public NSImageBitmap(NSImage image)
        {
            _image = image;
        }

        public override SizeF Size => _image?.Size.ToSizeF() ?? SizeF.Zero;

        public override object NativeBitmap => _image;

        protected override void DisposeNative()
        {
            _image?.Dispose();
            _image = null;
        }
    }
}
