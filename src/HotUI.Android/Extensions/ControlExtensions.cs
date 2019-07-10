using System;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Drawables;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public static partial class ControlExtensions
    {
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