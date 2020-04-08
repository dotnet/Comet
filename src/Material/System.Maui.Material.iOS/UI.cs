using System;
namespace System.Maui.Material.iOS
{
	public class UI
	{
		static bool _hasInitialized;

		public static void Init()
		{
			if (_hasInitialized) return;
			_hasInitialized = true;

			//Always init native before setting up custom renderers
			System.Maui.iOS.UI.Init();
			// Controls
			Registrar.Handlers.Register<Button, ButtonHandler>();
		}
	}
}
