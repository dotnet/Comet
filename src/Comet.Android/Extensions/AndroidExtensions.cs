using System;
using Android.App;
using Comet.Android.Controls;
using AView = Android.Views.View;
using ATextAlignment = Android.Views.TextAlignment;
using Comet.Internal;

namespace Comet.Android
{
	public static partial class AndroidExtensions
	{
		static AndroidExtensions()
		{
			Comet.Android.UI.Init();
		}

		/*public static Activity ToActivity(this HotPage hotPage)
        {
            if (hotPage == null)
                return null;
            var handler = hotPage.ViewHandler;
            if (handler == null)
            {
                handler = Registrar.Pages.GetHandler(hotPage.GetType()) as IViewContainer;
                hotPage.ViewHandler = handler;
                hotPage.ReBuildView();
            }

            var page = handler as IViewContainer;
            return page.Activity;
        }*/
		public static CometFragment ToFragment(this View view) => new CometFragment(view);

		public static AndroidViewHandler GetOrCreateViewHandler(this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null)
			{
				var builtView = view.GetView();
				handler = Registrar.Handlers.GetHandler(builtView.GetType());
				builtView.ViewHandler = handler;
			}
			var aView = handler as AndroidViewHandler;
			return aView;
		}

		public static AView ToView(this View view)
		{
			var handler = view.GetOrCreateViewHandler();
			return handler?.View;
		}

		public static global::Android.Graphics.Color ToColor(this Color color)
		{
			if (color == null)
				return global::Android.Graphics.Color.Black;

			var r = (int)(color.R * 255f);
			var g = (int)(color.G * 255f);
			var b = (int)(color.B * 255f);
			var a = (int)(color.A * 255f);
			return new global::Android.Graphics.Color(r, g, b, a);
		}


		public static Color ToColor(this global::Android.Graphics.Color color)
		{
			var r = color.R / 255f;
			var g = color.G / 255f;
			var b = color.B / 255f;
			var a = color.A / 255f;
			return new Color(r, g, b, a);
		}

		public static Color ToColor(this int colorInt)
			=> new global::Android.Graphics.Color(colorInt).ToColor();

		public static ATextAlignment ToAndroidTextAlignment(this TextAlignment? target)
		{
			if (target == null)
				return ATextAlignment.ViewStart;

			switch (target)
			{
				case TextAlignment.Natural:
					return ATextAlignment.TextStart;
				case TextAlignment.Left:
					return ATextAlignment.ViewStart;
				case TextAlignment.Right:
					return ATextAlignment.ViewEnd;
				case TextAlignment.Center:
					return ATextAlignment.Center;
				case TextAlignment.Justified:
					return ATextAlignment.Gravity;
				default:
					throw new ArgumentOutOfRangeException(nameof(target), target, null);
			}
		}
	}
}
