using Android.Content;
using HotUI.Android.Controls;

// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.Android.Handlers
{
    public class ImageHandler : AbstractControlHandler<Image, HUIImageView>
    {
        public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>(ViewHandler.Mapper)
        {
            [nameof(Image.Bitmap)] = MapBitmapProperty
        };

        public ImageHandler() : base(Mapper)
        {
        }

        protected override HUIImageView CreateView(Context context)
        {
            return new HUIImageView(context);
        }

        protected override void DisposeView(HUIImageView nativeView)
        {
        }

        public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
        {
            var nativeView = (HUIImageView) viewHandler.NativeView;
            nativeView.Bitmap = virtualView.Bitmap;
        }
    }
}