using Android.Content;
using Comet.Android.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Handlers
{
	public partial class ImageHandler : ViewHandler<Image, CometImageView>
	{
		public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>(ViewHandler.ViewMapper)
		{
			[nameof(Image.Bitmap)] = MapBitmapProperty
		};

		public ImageHandler() : base(Mapper)
		{
		}

		protected override CometImageView CreateNativeView()
		{
			return new CometImageView(Context);
		}

		public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
		{
			var nativeView = (CometImageView)viewHandler.NativeView;
			nativeView.Bitmap = virtualView.Bitmap?.CurrentValue;
			virtualView.InvalidateMeasurement();
		}
	}
}
