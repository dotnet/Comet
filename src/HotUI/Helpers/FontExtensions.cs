using System;
namespace HotUI {
	public static class FontExtensions {
		public static T SetFontSize<T> (this T view, int fontSize) where T : View
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
	}
}
