using System;
namespace Comet.Material.iOS
{
    public class UI
    {
        static bool _hasInitialized;

        public static void Init()
        {
            if (_hasInitialized) return;
            _hasInitialized = true;

            // Controls
            Registrar.Handlers.Register<Button, ButtonHandler>();
        }
    }
}
