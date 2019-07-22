using System;
using System.Diagnostics;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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

        internal string CurrentSource;

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
            try
            {
                var bitmap = virtualView.Bitmap;
                var source = 
                imageHandler.TypedNativeView.Source = bitmap.NativeBitmap as ImageSource;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            virtualView.InvalidateMeasurement();
        }
    }
}