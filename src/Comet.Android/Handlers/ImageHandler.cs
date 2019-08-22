using Android.Content;
using Comet.Android.Controls;

// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Android.Handlers
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

        protected override CUIImageView CreateView(Context context)
        {
            return new CUIImageView(context);
        }

        protected override void DisposeView(CUIImageView nativeView)
        {
        }

        public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
        {
            var nativeView = (CUIImageView) viewHandler.NativeView;
            nativeView.Bitmap = virtualView.Bitmap?.Get();
        }
    }
}
