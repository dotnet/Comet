using System;
using Comet.Skia.iOS;

namespace Comet.Skia
{
	public class Controls
	{
		static bool _hasInitialized;

		public static void Init()
		{
			if (_hasInitialized) return;
			_hasInitialized = true;
			Comet.iOS.UI.Init();
			UI.Init();

			var generic = typeof(SkiaControlHandler<>);

			Skia.Internal.Registration.RegisterReplacementViews(generic);
		}
	}
}
