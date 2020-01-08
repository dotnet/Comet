using System;

// ReSharper disable once CheckNamespace
namespace Comet
{
	public static class FontExtensions
	{
		public static FontAttributes GetFont(this View view, FontAttributes defaultFont)
		{
			var size = view.GetEnvironment<float?>(EnvironmentKeys.Fonts.Size) ?? defaultFont.Size;
			var name = view.GetEnvironment<string>(EnvironmentKeys.Fonts.Family) ?? defaultFont.Family;
			var weight = view.GetEnvironment<Weight?>(EnvironmentKeys.Fonts.Weight) ?? defaultFont.Weight;
			var italic = view.GetEnvironment<bool?>(EnvironmentKeys.Fonts.Italic) ?? defaultFont.Italic;
			return new FontAttributes
			{
				Size = size,
				Family = name,
				Italic = italic,
				Weight = weight,
			};
		}

		public static T Font<T>(this T view, FontAttributes value) where T : View
			=> view.FontFamily(value.Family)
			.FontSize(value.Size)
			.FontItalic(value.Italic)
			.FontWeight(value.Weight);

		public static T FontSize<T>(this T view, float value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Size, value, true);
		public static T FontFamily<T>(this T view, string value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Family, value, true, ControlState.Default);
		public static T FontItalic<T>(this T view, bool value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Italic, value, true);
		public static T FontWeight<T>(this T view, Weight value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Weight, value, true);

	}
}
