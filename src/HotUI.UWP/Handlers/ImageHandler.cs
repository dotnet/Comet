using System;
using System.Diagnostics;
using UWPImage = Windows.UI.Xaml.Controls.Image;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.UWP.Handlers
{
    public class ImageHandler : AbstractHandler<Image, UWPImage>
    {
        public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>()
        {
            [nameof(Image.Source)] = MapSourceProperty
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

        public static void MapSourceProperty(IViewHandler viewHandler, Image virtualView)
        {
            var imageHandler = (ImageHandler)viewHandler;
            UpdateSource(imageHandler, virtualView.Source);
        }

        public static async void UpdateSource(ImageHandler imageView, string source)
        {
            if (source == imageView.CurrentSource)
                return;

            imageView.CurrentSource = source;
            try
            {
                var image = await source.LoadImage();
                if (source == imageView.CurrentSource)
                    imageView.TypedNativeView.Source = image;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}