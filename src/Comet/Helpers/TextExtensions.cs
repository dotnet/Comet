using System;
using Microsoft.Maui;

// ReSharper disable once CheckNamespace
namespace Comet
{
	public static partial class TextExtensions
	{
		public static T TextAlignment<T>(this T view, Binding<TextAlignment?> alignment, bool cascades = true) where T : View =>
		view.SetEnvironment(EnvironmentKeys.Text.Alignment, alignment, cascades);
		public static T TextAlignment<T>(this T view, Func<TextAlignment?> alignment, bool cascades = true) where T : View =>
			view.TextAlignment((Binding<TextAlignment?>)alignment, cascades);

		public static TextAlignment? GetTextAlignment(this View view, TextAlignment? defaultValue = null)
		{
			var value = view.GetEnvironment<TextAlignment?>(view, EnvironmentKeys.Text.Alignment);
			return value ?? defaultValue;
		}
		public static TextAlignment? GetVerticalTextAlignment(this View view, TextAlignment? defaultValue = null)
		{
			var value = view.GetEnvironment<TextAlignment?>(view, EnvironmentKeys.Text.VerticalAlignment);
			return value ?? defaultValue;
		}

	}
}
