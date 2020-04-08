using Android.Content;
using System.Maui.Android.Controls;

// ReSharper disable MemberCanBePrivate.Global

namespace System.Maui.Android.Handlers
{
	public class ImageHandler : AbstractControlHandler<Image, MauiImageView>
	{
		public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>(ViewHandler.Mapper)
		{
			[nameof(Image.Bitmap)] = MapBitmapProperty
		};

		public ImageHandler() : base(Mapper)
		{
		}

		protected override MauiImageView CreateView(Context context)
		{
			return new MauiImageView(context);
		}

		protected override void DisposeView(MauiImageView nativeView)
		{
		}

		public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
		{
			var nativeView = (MauiImageView)viewHandler.NativeView;
			nativeView.Bitmap = virtualView.Bitmap?.CurrentValue;
			virtualView.InvalidateMeasurement();
		}
	}
}
