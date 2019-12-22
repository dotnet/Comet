using System;
using System.Diagnostics;
using CoreGraphics;
using Comet.Graphics;
using UIKit;

namespace Comet.iOS.Controls
{
	public class CUIImageView : UIImageView
	{
		private Bitmap _bitmap;

		public CUIImageView(CGRect frame) : base(frame)
		{

		}

		public Bitmap Bitmap
		{
			get => _bitmap;
			set
			{
				_bitmap = value;
				Image = _bitmap?.NativeBitmap as UIImage;
			}
		}

		public override CGSize SizeThatFits(CGSize size)
		{
			if (Image != null)
			{
				var maxX = Math.Min(Image.Size.Width, size.Width);
				var maxY = Math.Max(Image.Size.Height, size.Height);

				var fx = size.Width / maxX;
				var fy = size.Height / maxY;

				var factor = Math.Min(fx, fy);
				factor = Math.Min(factor, 1);

				return new CGSize(Image.Size.Width * factor, Image.Size.Height * factor);
			}

			return base.SizeThatFits(size);
		}

		public override void SizeToFit()
		{
			if (Image != null)
			{
				Frame = new CGRect(new CGPoint(), Image.Size);
			}
			else
			{
				base.SizeToFit();
			}
		}
	}
}
