using System;
namespace HotUI {
	public static class ColorExtensions 
	{
		/// <summary>
		/// Set the color
		/// </summary>
		/// <param name="view"></param>
		/// <param name="color"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Color<T> (this T view, Color color) where T : View
		{
			view.SetEnvironment (EnvironmentKeys.Colors.Color, color);
			return view;
		}
		
		public static Color GetColor (this View view,Color defaultColor)
		{
			var color = view.GetEnvironment<Color> (EnvironmentKeys.Colors.Color);
			return color ?? defaultColor;
		}
	}
}
