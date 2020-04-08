using System;
using System.Maui.Skia.Android;

namespace System.Maui.Skia
{
	public static class Controls
	{

		static bool _hasInitialized;

		public static void Init()
		{
			if (_hasInitialized) return;
			_hasInitialized = true;
			System.Maui.Android.UI.Init();
			UI.Init();

			var generic = typeof(SkiaControlHandler<>);
			Skia.Internal.Registration.RegisterReplacementViews(generic);
		}
	}
}
