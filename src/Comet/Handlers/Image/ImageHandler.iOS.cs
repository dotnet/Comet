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
		public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>(ViewHandler.ViewMapper)
		{
			[nameof(Image.Bitmap)] = MapBitmapProperty
		};

		public ImageHandler() : base(Mapper)
		{

		}

		protected override CUIImageView CreateNativeView()
		{
			return new CUIImageView(new CGRect(0, 0, 44, 44));
		}


		public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
		{
			var nativeView = (CUIImageView)viewHandler.NativeView;
			nativeView.Bitmap = virtualView.Bitmap?.CurrentValue;
			virtualView.InvalidateMeasurement();
		}
	}
}
