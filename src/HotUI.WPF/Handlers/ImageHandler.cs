using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WPFImage = System.Windows.Controls.Image;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.WPF.Handlers
{
    public class ImageHandler : AbstractHandler<Image, WPFImage>
    {
        public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>()
        {
            [nameof(Image.Source)] = MapSourceProperty
        };
        
        internal string CurrentSource;

        public ImageHandler() : base(Mapper)
        {
        }

        protected override WPFImage CreateView() => new WPFImage();

        protected override void DisposeView(WPFImage nativeView)
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
