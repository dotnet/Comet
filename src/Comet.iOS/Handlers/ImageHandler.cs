using CoreGraphics;
using Comet.iOS.Controls;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.iOS.Handlers
{
	public class ImageHandler : AbstractControlHandler<Image, CUIImageView>
	{
		public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>(ViewHandler.Mapper)
		{
			[nameof(Image.Bitmap)] = MapBitmapProperty
		};

		public ImageHandler() : base(Mapper)
		{

		}

		protected override CUIImageView CreateView()
		{
			return new CUIImageView(new CGRect(0, 0, 44, 44));
		}

		protected override void DisposeView(CUIImageView nativeView)
		{

		}

		public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
		{
			var nativeView = (CUIImageView)viewHandler.NativeView;
			nativeView.Bitmap = virtualView.Bitmap?.CurrentValue;
			virtualView.InvalidateMeasurement();
		}
	}
}
