using System;
using Microsoft.Maui;

// ReSharper disable once CheckNamespace
namespace Comet
{
	public static class FontExtensions
	{
		public static Font GetFont(this View view, Font? defaultFont)
		{
			var size = view.GetEnvironment<double?>(EnvironmentKeys.Fonts.Size) ?? defaultFont?.FontSize;
			var name = view.GetEnvironment<string>(EnvironmentKeys.Fonts.Family) ?? defaultFont?.FontFamily;
			var attributes = view.GetEnvironment<FontAttributes?>(EnvironmentKeys.Fonts.Attributes) ?? defaultFont?.FontAttributes;
			Font font = Font.Default;
			if (!string.IsNullOrWhiteSpace(name))
				font = Font.OfSize(name, size ?? font.FontSize);
			if (attributes != null)
				font = font.WithAttributes(attributes.Value);
			return font;
		}

		public static T FontSize<T>(this T view, double value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Size, value, true);
		public static T FontFamily<T>(this T view, string value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Family, value, true, ControlState.Default);
		public static T SetFontAttributes<T>(this T view, FontAttributes value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Attributes, value, true);

	}
}
