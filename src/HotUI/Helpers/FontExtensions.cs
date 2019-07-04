using System;
namespace HotUI {
	public static class FontExtensions 
	{
		/// <summary>
		/// Set the font size.
		/// </summary>
		/// <param name="view"></param>
		/// <param name="fontSize"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T FontSize<T> (this T view, int fontSize) where T : View
		{
			view.SetEnvironment (EnvironmentKeys.Fonts.FontSize, fontSize);
			return view;
		}
		
		public static int GetFontSize (this View view,int defaultSize)
		{
			var size = view.GetEnvironment<int?> (EnvironmentKeys.Fonts.FontSize);
			if (size.HasValue) {
				return size.Value;
			}
			return defaultSize;
		}
		
		/// <summary>
		/// Set the font size.
		/// </summary>
		/// <param name="view"></param>
		/// <param name="fontSize"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Font<T> (this T view, Font font) where T : View
		{
			view.SetEnvironment (EnvironmentKeys.Fonts.Font, font);
			return view;
		}
		
		public static Font GetFont (this View view, Font defaultFont)
		{
			var font = view.GetEnvironment<Font> (EnvironmentKeys.Fonts.Font);
			return font ?? defaultFont;
		}
	}
}
