using Android.Content;
using Comet.Android.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Handlers
{
	public partial class ImageHandler : ViewHandler<Image, CometImageView>
	{
	
		protected override CometImageView CreateNativeView()
		{
			return new CometImageView(Context);
		}

		public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
		{
			var nativeView = (CometImageView)viewHandler.NativeView;
			nativeView.Bitmap = virtualView.Bitmap?.CurrentValue;
			((IView)virtualView).InvalidateMeasure();
		}
	}
}
