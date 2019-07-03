using System;
namespace HotUI
{
	public static class LayoutExtensions
    {
        public static T Padding<T>(this T view) where T : View
        {
            var defaultThickness = new Thickness(10);
            return view.Padding(defaultThickness);
        }

        public static T Padding<T> (this T view, Thickness padding) where T : View
		{
			view.SetEnvironment (EnvironmentKeys.Layout.Padding, padding);
			return view;
		}

		public static Thickness GetPadding (this View view, Thickness defaultPadding)
		{
			var padding = view.GetEnvironment<Thickness?> (EnvironmentKeys.Layout.Padding);
			if (padding != null) {
				return (Thickness)padding;
			}
			return defaultPadding;
		}
	}
}
