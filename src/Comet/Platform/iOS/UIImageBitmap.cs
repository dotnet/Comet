using Comet.Graphics;
using Microsoft.Maui.Graphics;
using UIKit;
using Bitmap = Comet.Graphics.Bitmap;
using Microsoft.Maui.Graphics.CoreGraphics;
namespace Comet.iOS.Graphics
{
	public class UIImageBitmap : Bitmap
	{
		private UIImage _image;

		public UIImageBitmap(UIImage image)
		{
			_image = image;
		}

		public override Size Size => _image?.Size.AsSizeF() ?? Size.Zero;

		public override object NativeBitmap => _image;

		protected override void DisposeNative()
		{
			_image?.Dispose();
			_image = null;
		}
	}
}
