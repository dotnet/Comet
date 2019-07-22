using Windows.UI.Xaml.Media;
using UWPImage = Windows.UI.Xaml.Controls.Image;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.UWP.Handlers
{
    public class ImageHandler : AbstractHandler<Image, UWPImage>
    {
        public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>()
        {
            [nameof(Image.Bitmap)] = MapBitmapProperty
        };

        public ImageHandler() : base(Mapper)
        {

        }

        protected override UWPImage CreateView()
        {
            return new UWPImage();
        }

        protected override void DisposeView(UWPImage nativeView)
        {

        }

        public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
        {
            var imageHandler = (ImageHandler)viewHandler;
            var bitmap = virtualView.Bitmap;
            var nativeBitmap = (ImageSource)bitmap?.NativeBitmap;
            imageHandler.TypedNativeView.Source = nativeBitmap;
            virtualView.InvalidateMeasurement();
        }
    }
}