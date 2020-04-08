using System;
using FFImageLoading;
using System.Threading.Tasks;
using System.Diagnostics;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using FFImageLoading.Drawables;
using AView = Android.Views.View;

namespace System.Maui.Android.Controls
{
	public class MauiImageView : ImageView
	{
		private Image _image;
		private Maui.Graphics.Bitmap _bitmap;

		public MauiImageView(Context context) : base(context)
		{
		}

		public Maui.Graphics.Bitmap Bitmap
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
