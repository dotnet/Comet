using System;
using System.Maui.Skia.Mac;

namespace System.Maui.Skia
{
	public class Controls
	{
		static bool _hasInitialized;

		public static void Init()
		{
			if (_hasInitialized) return;
			_hasInitialized = true;
			System.Maui.Mac.UI.Init();
			UI.Init();

			var generic = typeof(SkiaControlHandler<>);

			Skia.Internal.Registration.RegisterReplacementViews(generic);
		}
	}
}
