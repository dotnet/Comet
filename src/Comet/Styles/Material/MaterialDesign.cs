using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.Styles
{
    public static partial class MaterialDesign
    {
        public static void Apply() => Apply(PrimaryColorPalette, SecondaryColorPalette);
        
        public static T Apply<T>(this T view ) where T :View
        {
            return view;
        }

        public static void ApplyButtonStyles(ColorPalette colorPalette)
        {
            //Set the Text color
            View.SetGlobalEnvironment(typeof(Button), EnvironmentKeys.Colors.Color, colorPalette.P900);
            //Set the BorderStyle
            //View.SetGlobalEnvironment(typeof(Button), EnvironmentKeys.Colors.Color, colorPalette.P900);
        }

        public static void ApplyNavbarStyles(ColorPalette colorPalette)
        {
            View.SetGlobalEnvironment(EnvironmentKeys.Navigation.BackgroundColor, colorPalette.P500);
            View.SetGlobalEnvironment(EnvironmentKeys.Navigation.TextColor, Color.White);
        }

        public static void Apply(ColorPalette colorPalette) => Apply(colorPalette, colorPalette);

        public static void Apply(ColorPalette primary, ColorPalette secondary)
        {
            ApplyButtonStyles(primary);
            ApplyNavbarStyles(primary);
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
