using System;
namespace HotUI {
	public static class ColorExtensions {
		public static T SetColor<T> (this T view, Color color) where T : View
		{
			view.SetEnvironment (EnvironmentKeys.Colors.Color, color);
			return view;
		}
		public static Color GetColor (this View view,Color defaultColor)
		{
			var color = view.GetEnvironment<Color> (EnvironmentKeys.Colors.Color);
			if (color != null) {
				return color;
			}
			return defaultColor;
		}
	}
}
