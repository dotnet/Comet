using System;
namespace Comet
{
    /// <summary>
    /// A design to use for fonts.
    /// </summary>
    public enum Design
    {
        Default,
        Monospaced,
        Rounded,
        Serif,
    }
    
    public static class DesignExtensions
    {
        public static string FontName(this Design design)
        {
            switch (design)
            {
                case Design.Monospaced:
                    return Device.FontService.MonospacedFontName;
                case Design.Rounded:
                    return Device.FontService.RoundedFontName;
                case Design.Serif:
                    return Device.FontService.SerifFontName;
                default:
                    return Device.FontService.SystemFontName;
            }
        }
    }
}
