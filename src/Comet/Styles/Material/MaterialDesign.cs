using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.Styles
{
    public  static partial class MaterialDesign
    {
        public static void Apply()
        {
            SetColorPalette(PrimaryColorPalette, SecondaryColorPalette);
        }
        public static T Apply<T>(this T view ) where T :View
        {
            return view;
        }
        public static void SetColorPalette(ColorPalette primary, ColorPalette secondary)
        {

        }

        public static ColorPalette PrimaryColorPalette
        {
            get => View.GetGlobalEnvironment<ColorPalette>(nameof(PrimaryColorPalette)) ?? ColorPalettes.Blue;
            set => View.SetGlobalEnvironment(nameof(PrimaryColorPalette), value);
        }
        public static ColorPalette SecondaryColorPalette
        {
            get => View.GetGlobalEnvironment<ColorPalette>(nameof(SecondaryColorPalette)) ?? ColorPalettes.Blue;
            set => View.SetGlobalEnvironment(nameof(SecondaryColorPalette), value);
        }


    }
}
