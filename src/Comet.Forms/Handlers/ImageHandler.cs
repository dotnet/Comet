using FImage = Xamarin.Forms.Image;
using HImage = Comet.Image;

namespace Comet.Forms.Handlers
{
    public class ImageHandler : AbstractControlHandler<HImage, FImage>
    {
        public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>(ViewHandler.Mapper)
        {
            [nameof(Image.Bitmap)] = MapBitmapProperty
        };

        public ImageHandler() : base(Mapper)
        {
        }
        protected override FImage CreateView()
        {
            throw new System.NotImplementedException();
        }

        protected override void DisposeView(FImage nativeView)
        {
            
        }

        public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
        {
            var nativeView = (FImage)viewHandler.NativeView;
            nativeView.Source = virtualView.Bitmap?.ToImageSource();
        }
    }
}
