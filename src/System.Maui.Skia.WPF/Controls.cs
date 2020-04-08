using System;
using System.Maui.Skia.WPF;

namespace System.Maui.Skia
{
	public class Controls
	{
		static bool _hasInitialized;

		public static void Init()
		{
			if (_hasInitialized) return;
			_hasInitialized = true;
			UI.Init();

			var generic = typeof(SkiaControlHandler<>);

			Skia.Internal.Registration.RegisterReplacementViews(generic);
		}
	}
}
