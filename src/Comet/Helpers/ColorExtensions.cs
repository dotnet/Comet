using System;

// ReSharper disable once CheckNamespace
namespace Comet
{
    public static class ColorExtensions
    {

        public static Color WithAlpha(this Color color, float alpha)
            => new Color(color.R, color.G, color.B, alpha);
   
        /// <summary>
        /// Set the color
        /// </summary>
        /// <param name="view"></param>
        /// <param name="color"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Color<T> (this T view, Color color) where T: View
        {
            view.SetEnvironment(EnvironmentKeys.Colors.Color, color,false);
            return view;
        }
        public static T Color<T>(this T view, Type type, Color color) where T : View
        {
            view.SetEnvironment(type,EnvironmentKeys.Colors.Color, color,true);
            return view;
        }

        public static Color GetColor<T>(this T view, Color defaultColor) where T : View
        {
            var color = view.GetEnvironment<Color>(EnvironmentKeys.Colors.Color);
            return color ?? defaultColor;
        }

        public static Color GetColor<T>(this T view, Type type, Color defaultColor) where T : View
        {
            var color = view.GetEnvironment<Color>(type,EnvironmentKeys.Colors.Color);
            return color ?? defaultColor;
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
            view.SetEnvironment(EnvironmentKeys.Colors.BackgroundColor, color, cascades);
            return view;
        }

        /// <summary>
        /// Set the background color by hex value
        /// </summary>
        /// <param name="view"></param>
        /// <param name="color"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Background<T>(this T view, string color, bool cascades = false) where T : View
        {
            view.SetEnvironment(EnvironmentKeys.Colors.BackgroundColor, new Color(color), cascades);
            return view;
        }

        /// <summary>
        /// Set the background color
        /// </summary>
        /// <param name="view"></param>
        /// <param name="color"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Background<T>(this T view, Type type, Color color) where T : View
        {
            view.SetEnvironment(type, EnvironmentKeys.Colors.BackgroundColor, color, true);
            return view;
        }

        public static Color GetBackgroundColor(this View view,Type type, Color defaultColor = null)
        {
            var color = view?.GetEnvironment<Color>(type,EnvironmentKeys.Colors.BackgroundColor);
            return color ?? defaultColor;
        }

        public static Color GetBackgroundColor(this View view, Color defaultColor = null)
        {
            var color = view?.GetEnvironment<Color>(EnvironmentKeys.Colors.BackgroundColor);
            return color ?? defaultColor;
        }

        public static Color GetNavigationBackgroundColor(this View view, Color defaultColor = null)
        {
            var color = view.GetEnvironment<Color>(EnvironmentKeys.Navigation.BackgroundColor);
            return color ?? defaultColor;
        }
        public static Color GetNavigationTextColor(this View view, Color defaultColor = null)
        {
            var color = view.GetEnvironment<Color>(EnvironmentKeys.Navigation.TextColor);
            return color ?? defaultColor;
        }

    }
}
