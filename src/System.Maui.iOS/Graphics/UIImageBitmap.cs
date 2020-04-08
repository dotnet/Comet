using System.Maui.Graphics;
using System.Drawing;
using UIKit;
using Bitmap = System.Maui.Graphics.Bitmap;

namespace System.Maui.iOS.Graphics
{
	public class UIImageBitmap : Bitmap
	{
		private UIImage _image;

		public UIImageBitmap(UIImage image)
		{
			_image = image;
		}

		public override SizeF Size => _image?.Size.ToSizeF() ?? SizeF.Empty;

		public override object NativeBitmap => _image;

		protected override void DisposeNative()
		{
			_image?.Dispose();
			_image = null;
		}
	}
}
