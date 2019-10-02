namespace Comet.Skia.Android
{
	public static class UI
    {
		static bool _hasInitialized;

		public static void Init ()
		{
			if (_hasInitialized) return;
			_hasInitialized = true;

            // Controls
            Registrar.Handlers.Register<DrawableControl, DrawableControlHandler> ();
            Registrar.Handlers.Register<SkiaView, SkiaViewHandler> ();
		}
    }
}
