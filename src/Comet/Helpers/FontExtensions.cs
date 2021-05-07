using System;
using Microsoft.Maui;

// ReSharper disable once CheckNamespace
namespace Comet
{
	public static class FontExtensions
	{
		public static Font GetFont(this View view, Font? defaultFont)
		{
			Font font = Font.Default;
			var  size = view.GetEnvironment<double?>(EnvironmentKeys.Fonts.Size) ?? defaultFont?.FontSize ?? font.FontSize;
			var name = view.GetEnvironment<string>(EnvironmentKeys.Fonts.Family) ?? defaultFont?.FontFamily ?? font.FontFamily;
			var weight = view.GetEnvironment<FontWeight?>(EnvironmentKeys.Fonts.Weight) ?? defaultFont?.Weight ?? Microsoft.Maui.FontWeight.Regular;
			var slant = view.GetEnvironment<FontSlant?>(EnvironmentKeys.Fonts.Slant) ?? Microsoft.Maui.FontSlant.Default;
			if (!string.IsNullOrWhiteSpace(name))
				return Font.OfSize(name, size, weight, slant);
			//else if (size > 0)
				return Font.SystemFontOfSize(size, weight, slant);
			//return font;
		}

		public static T FontSize<T>(this T view, double value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Size, value, true);
		public static T FontWeight<T>(this T view, FontWeight value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Weight, value, true);
		public static T FontFamily<T>(this T view, string value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Family, value, true, ControlState.Default);
		public static T FontSlant<T>(this T view, FontSlant value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Slant, value, true, ControlState.Default);
	}
}
