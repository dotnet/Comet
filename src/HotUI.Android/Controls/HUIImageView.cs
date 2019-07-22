using System;
using FFImageLoading;
using System.Threading.Tasks;
using System.Diagnostics;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using FFImageLoading.Drawables;
using AView = Android.Views.View;

namespace HotUI.Android.Controls
{
    public class HUIImageView : ImageView
    {
        private Image _image;
        private HotUI.Graphics.Bitmap _bitmap;

        public HUIImageView(Context context) : base(context)
        {
        }
        
        public HotUI.Graphics.Bitmap Bitmap
        {
            get => _bitmap;
            set
            {
                _bitmap = value;
                SetImageBitmap(_bitmap.NativeBitmap as Bitmap);
            }
        }
    }
}