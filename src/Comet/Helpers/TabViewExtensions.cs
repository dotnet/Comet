using System;
namespace Comet
{
	public static class TabViewExtensions
	{
		public static T TabIcon<T>(this T view, Binding<string> image) where T : View
			=> view.SetEnvironment(EnvironmentKeys.TabView.Image, image);
		public static T TabIcon<T>(this T view, Func<string> image) where T : View
			=> view.TabIcon((Binding<string>)image);

		public static T TabText<T>(this T view, Binding<string> text) where T : View
			=> view.SetEnvironment(EnvironmentKeys.TabView.Title, text);
		public static T TabText<T>(this T view, Func<string> text) where T : View
			=> view.TabText((Binding<string>)text);

		public static T Tab<T>(this T view, Binding<string> text, Binding<string> image) where T : View
			=> view.TabIcon(image).TabIcon(text);
		public static T Tab<T>(this T view, Func<string> text, Func<string> image) where T : View
			=> view.TabIcon(image).TabIcon(text);

	}
}
