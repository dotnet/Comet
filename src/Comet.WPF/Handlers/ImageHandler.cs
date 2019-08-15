using System.Windows.Media.Imaging;
using WPFImage = System.Windows.Controls.Image;
// ReSharper disable ClassNeverInstantiated.Global

namespace Comet.WPF.Handlers
{
    public class ImageHandler : AbstractControlHandler<Image, WPFImage>
    {
        public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>()
        {
            [nameof(Image.Bitmap)] = MapBitmapProperty
        };

        public ImageHandler() : base(Mapper)
        {
        }

        protected override WPFImage CreateView() => new WPFImage();

        protected override void DisposeView(WPFImage nativeView)
        {

        }

        public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
        {
            var imageHandler = (ImageHandler)viewHandler;
            var bitmap = (BitmapImage)virtualView.Bitmap?.Get()?.NativeBitmap;
            imageHandler.TypedNativeView.Source = bitmap;
            imageHandler.VirtualView.InvalidateMeasurement();
        }
    }
}
