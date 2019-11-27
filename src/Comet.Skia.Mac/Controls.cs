using System;
using Comet.Skia.Mac;

namespace Comet.Skia
{
	public class Controls
	{
		static bool _hasInitialized;

		public static void Init()
		{
			if (_hasInitialized) return;
			_hasInitialized = true;
			Comet.Mac.UI.Init();
			UI.Init();

			var generic = typeof(SkiaControlHandler<>);

			Skia.Internal.Registration.RegisterReplacementViews(generic);
		}
	}
}
