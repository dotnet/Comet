using System;
using FFImageLoading;
using System.Threading.Tasks;
using System.Diagnostics;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using FFImageLoading.Drawables;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class ImageHandler : ImageView, IView
    {
        public ImageHandler() : base(AndroidContext.CurrentContext)
        {
        }
        
        public AView View => throw new NotImplementedException();

        private Image image;
        public void Remove(View view)
        {
            image = null;
        }

        public void SetView(View view)
        {
            image = view as Image;
            this.UpdateProperties(image);
        }

        public void UpdateValue(string property, object value)
        {
            
        }

        string currentSource;

        public async void UpdateSource(string source)
        {
            if (source == currentSource)
                return;
            currentSource = source;
            try
            {
                var image = await source.LoadImage();
                if (source == currentSource)
                    this.SetImageBitmap(image.Bitmap);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }

    public static partial class ControlExtensions
    {
        public static void UpdateProperties(this ImageHandler view, Image hView)
        {
            view.UpdateSource(hView.Source);
            view.UpdateBaseProperties(hView);
        }

        public static bool UpdateProperty(this ImageHandler view, string property, object value)
        {
            switch (property)
            {
                case nameof(Image.Source):
                    view.UpdateSource((string) value);
                    return true;
            }

            return view.UpdateBaseProperty(property, value);
        }

        public static Task<SelfDisposingBitmapDrawable> LoadImage(this string source)
        {
            var isUrl = Uri.IsWellFormedUriString(source, UriKind.RelativeOrAbsolute);
            if (isUrl)
                return LoadImageAsync(source);
            return LoadFileAsync(source);
        }

        private static Task<SelfDisposingBitmapDrawable> LoadImageAsync(string urlString)
        {
            return ImageService.Instance
                .LoadUrl(urlString)
                .AsBitmapDrawableAsync();
        }

        private static Task<SelfDisposingBitmapDrawable> LoadFileAsync(string filePath)
        {
            return ImageService.Instance
                .LoadFile(filePath)
                .AsBitmapDrawableAsync();
        }
    }
}