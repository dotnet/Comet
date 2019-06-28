using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using UWPImage = Windows.UI.Xaml.Controls.Image;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.UWP.Handlers
{
    public class ImageHandler : IUIElement
    {
        private static readonly PropertyMapper<Image, ImageHandler> Mapper = new PropertyMapper<Image, ImageHandler>()
        {
            [nameof(Image.Source)] = MapSourceProperty
        };

        internal readonly UWPImage NativeImageView;
        private Image _image;
        internal string CurrentSource;

        public ImageHandler()
        {
            NativeImageView = new UWPImage();
        }

        public UIElement View => NativeImageView;
        
        public void Remove(View view)
        {
        }

        public void SetView(View view)
        {
            _image = view as Image;
            Mapper.UpdateProperties(this, _image);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _image, property);
        }
        
        public static bool MapSourceProperty(ImageHandler nativeView, Image virtualView)
        {
            nativeView.UpdateSource(virtualView.Source);
            return true;
        }
    }

    public static partial class ControlExtensions
    {
        public static async void UpdateSource(this ImageHandler imageView, string source)
        {
            if (source == imageView.CurrentSource)
                return;
            
            imageView.CurrentSource = source;
            try
            {
                var image = await source.LoadImage();
                if (source == imageView.CurrentSource)
                    imageView.NativeImageView.Source = image;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
       
        public static Task<ImageSource> LoadImage(this string source)
        {
            throw new NotImplementedException();
        }

        private static Task<ImageSource> LoadImageAsync(string urlString)
        {
            throw new NotImplementedException();

        }

        private static Task<ImageSource> LoadFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}