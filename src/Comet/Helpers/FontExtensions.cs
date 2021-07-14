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

		public static T FontSize<T>(this T view, Binding<double> value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Size, value, true);
		public static T FontSize<T>(this T view, Func<double> value) where T : View
			=> view.FontSize((Binding<double>)value);

		public static T FontWeight<T>(this T view, Binding<FontWeight> value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Weight, value, true);
		public static T FontWeight<T>(this T view, Func<FontWeight> value) where T : View
			=> view.FontWeight((Binding<FontWeight>)value);

		public static T FontFamily<T>(this T view, Binding<string> value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Family, value, true, ControlState.Default);
		public static T FontFamily<T>(this T view, Func<string> value) where T : View
			=> view.FontFamily((Binding<string>)value);

		public static T FontSlant<T>(this T view, Binding<FontSlant> value) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Fonts.Slant, value, true, ControlState.Default);
		public static T FontSlant<T>(this T view, Func<FontSlant> value) where T : View
			 => view.FontSlant((Binding<FontSlant>)value);
	}
}
