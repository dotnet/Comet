using CoreGraphics;
using HotUI.iOS.Controls;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS.Handlers
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

        protected override HUIImageView CreateView()
        {
            return new HUIImageView(new CGRect(0, 0, 44, 44));
        }

        protected override void DisposeView(HUIImageView nativeView)
        {
            
        }

        public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
        {
            var nativeView = (HUIImageView) viewHandler.NativeView;
            nativeView.Bitmap = virtualView.Bitmap;
            virtualView.InvalidateMeasurement();
        }
    }
}