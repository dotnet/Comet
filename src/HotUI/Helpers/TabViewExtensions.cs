using System;
namespace HotUI
{
    public static class TabViewExtensions
    {
        public static T TabIcon<T>(this T view, string image) where T : View
            => view.SetEnvironment(EnvironmentKeys.TabView.Image, image);
        public static T TabText<T>(this T view, string text) where T : View
            => view.SetEnvironment(EnvironmentKeys.TabView.Title, text);
        public static T Tab<T>(this T view, string text, string image) where T : View
            => view.TabIcon(image).TabIcon(text);

    }
}
