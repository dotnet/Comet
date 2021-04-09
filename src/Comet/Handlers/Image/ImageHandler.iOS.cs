using CoreGraphics;
using Comet.iOS.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Handlers
{
	public partial class ImageHandler : ViewHandler<Image, CUIImageView>
	{
		protected override CUIImageView CreateNativeView()
		{
			return new CUIImageView(new CGRect(0, 0, 44, 44));
		}


		public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
		{
			var nativeView = (CUIImageView)viewHandler.NativeView;
			nativeView.Bitmap = virtualView.Bitmap?.CurrentValue;
			((IView)virtualView).InvalidateMeasure();
		}
	}
}
