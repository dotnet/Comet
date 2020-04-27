using Android.Content;
using Comet.Android.Controls;

// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Android.Handlers
{
	public class ImageHandler : AbstractControlHandler<Image, CometImageView>
	{
		public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>(ViewHandler.Mapper)
		{
			[nameof(Image.Bitmap)] = MapBitmapProperty
		};

		public ImageHandler() : base(Mapper)
		{
		}

		protected override CometImageView CreateView(Context context)
		{
			return new CometImageView(context);
		}

		protected override void DisposeView(CometImageView nativeView)
		{
		}

		public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
		{
			var nativeView = (CometImageView)viewHandler.NativeView;
			nativeView.Bitmap = virtualView.Bitmap?.CurrentValue;
			virtualView.InvalidateMeasurement();
		}
	}
}
