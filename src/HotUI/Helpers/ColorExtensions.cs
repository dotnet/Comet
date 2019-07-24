using System;

// ReSharper disable once CheckNamespace
namespace HotUI
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Set the color
        /// </summary>
        /// <param name="view"></param>
        /// <param name="color"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Text Color (this Text view, Color color)
        {
            view.SetEnvironment(EnvironmentKeys.Text.Color, color);
            return view;
        }

        public static Color GetColor(this Text view, Color defaultColor)
        {
            var color = view.GetEnvironment<Color>(EnvironmentKeys.Text.Color);
            return color ?? defaultColor;
        }
        /// <summary>
        /// Set the color
        /// </summary>
        /// <param name="view"></param>
        /// <param name="color"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TextField Color (this TextField view, Color color)
        {
            view.SetEnvironment(EnvironmentKeys.TextField.Color, color);
            return view;
        }

        public static Color GetColor(this TextField view, Color defaultColor)
        {
            var color = view.GetEnvironment<Color>(EnvironmentKeys.TextField.Color);
            return color ?? defaultColor;
        }

        /// <summary>
        /// Set the color
        /// </summary>
        /// <param name="view"></param>
        /// <param name="color"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Button TextColor(this Button view, Color color)
        {
            view.SetEnvironment(EnvironmentKeys.Button.TextColor, color);
            return view;
        }

        public static Color GetTextColor(this Button view, Color defaultColor)
        {
            var color = view.GetEnvironment<Color>(EnvironmentKeys.Button.TextColor);
            return color ?? defaultColor;
        }


        public static T TextColor<T>(this T view, Color color) where T: View
        {
            view.SetEnvironment(EnvironmentKeys.Button.TextColor, color);
            view.SetEnvironment(EnvironmentKeys.Text.Color, color);
            view.SetEnvironment(EnvironmentKeys.TextField.Color, color);
            return view;
        }

        /// <summary>
        /// Set the background color
        /// </summary>
        /// <param name="view"></param>
        /// <param name="color"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Background<T>(this T view, Color color, bool cascades = false) where T : View
        {
            view.SetEnvironment(EnvironmentKeys.Colors.BackgroundColor, color,cascades);
            return view;
        }

        public static Color GetBackgroundColor(this View view, Color defaultColor = null)
        {
            var color = view.GetEnvironment<Color>(EnvironmentKeys.Colors.BackgroundColor);
            return color ?? defaultColor;
        }
    }
}