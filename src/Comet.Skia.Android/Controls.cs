using System;
using Comet.Skia.Android;

namespace Comet.Skia
{
	public static class Controls
	{

		static bool _hasInitialized;

		public static void Init()
		{
			if (_hasInitialized) return;
			_hasInitialized = true;
			Comet.Android.UI.Init();
			UI.Init();

			var generic = typeof(SkiaControlHandler<>);
			Skia.Internal.Registration.RegisterReplacementViews(generic);
		}
	}
}
