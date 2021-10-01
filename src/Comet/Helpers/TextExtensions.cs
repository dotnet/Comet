using System;
using Microsoft.Maui;

// ReSharper disable once CheckNamespace
namespace Comet
{
	public static partial class TextExtensions
	{
		public static T HorizontalTextAlignment<T>(this T view, Binding<TextAlignment?> alignment, bool cascades = true) where T : View =>
		view.SetEnvironment(EnvironmentKeys.Text.HorizontalAlignment, alignment, cascades);
		public static T HorizontalTextAlignment<T>(this T view, Func<TextAlignment?> alignment, bool cascades = true) where T : View =>
			view.HorizontalTextAlignment((Binding<TextAlignment?>)alignment, cascades);
		public static T VerticalTextAlignment<T>(this T view, Binding<TextAlignment?> alignment, bool cascades = true) where T : View =>
		view.SetEnvironment(EnvironmentKeys.Text.VerticalAlignment, alignment, cascades);
		public static T VerticalTextAlignment<T>(this T view, Func<TextAlignment?> alignment, bool cascades = true) where T : View =>
			view.VerticalTextAlignment((Binding<TextAlignment?>)alignment, cascades);


	}
}
