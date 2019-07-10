using System;
using FFImageLoading;
using System.Threading.Tasks;
using System.Diagnostics;
using Android.Content;
using Android.Widget;
using FFImageLoading.Drawables;
using AView = Android.Views.View;

namespace HotUI.Android.Controls
{
    public class HUIImageView : ImageView
    {
        private Image _image;
        private string _source;

        public HUIImageView(Context context) : base(context)
        {
        }
        
        public string Source
        {
            get => _source;
            set => UpdateSource(value);
        }

        public async void UpdateSource(string source)
        {
            if (source == _source)
                return;
            
            _source = source;
            try
            {
                var image = await source.LoadImage();
                if (source == _source)
                    SetImageBitmap(image.Bitmap);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}