using System;
using FFImageLoading;
using System.Threading.Tasks;
using System.Diagnostics;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using FFImageLoading.Drawables;
using AView = Android.Views.View;

namespace Comet.Android.Controls
{
    public class CUIImageView : ImageView
    {
        private Image _image;
        private Comet.Graphics.Bitmap _bitmap;

        public CUIImageView(Context context) : base(context)
        {
        }
        
        public Comet.Graphics.Bitmap Bitmap
        {
            get => _bitmap;
            set
            {
                _bitmap = value;
                SetImageBitmap(_bitmap?.NativeBitmap as Bitmap);
            }
        }
    }
}
