using System;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Drawables;
using System.Maui.Android.Controls;
using AView = Android.Views.View;
using MotionEvent = Android.Views.MotionEvent;
using MotionEventActions = Android.Views.MotionEventActions;

namespace System.Maui.Android
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

		public static MauiTouchGestureListener GetGestureListener(this AndroidViewHandler handler)
			=> handler.GestureListener ?? (handler.GestureListener = new MauiTouchGestureListener(handler.View));

		public static bool IsComplete(this MotionEvent e)
		{
			switch (e.Action)
			{
				case MotionEventActions.Cancel:
				case MotionEventActions.Outside:
				case MotionEventActions.PointerUp:
				case MotionEventActions.Up:
					return true;
				default:
					return false;
			}
		}
	}
}
