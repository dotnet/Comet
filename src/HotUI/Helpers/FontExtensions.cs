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
