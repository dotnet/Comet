using Comet.Styles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Comet
{
	public static class StyleExtensions
	{
		public static T StyleId<T>(this T view, string styleId) where T : View
		{
			view.StyleId = styleId;
			return view;
		}
		public static T StyleAsH1<T>(this T text) where T : Text
		{
			text.StyleId = EnvironmentKeys.Text.Style.H1;
			return text;
		}

		public static T StyleAsH2<T>(this T text) where T : Text
		{
			text.StyleId = EnvironmentKeys.Text.Style.H2;
			return text;
		}

		public static T StyleAsH3<T>(this T text) where T : Text
		{
			text.StyleId = EnvironmentKeys.Text.Style.H3;
			return text;
		}

		public static T StyleAsH4<T>(this T text) where T : Text
		{
			text.StyleId = EnvironmentKeys.Text.Style.H4;
			return text;
		}

		public static T StyleAsH5<T>(this T text) where T : Text
		{
			text.StyleId = EnvironmentKeys.Text.Style.H5;
			return text;
		}

		public static T StyleAsH6<T>(this T text) where T : Text
		{
			text.StyleId = EnvironmentKeys.Text.Style.H6;
			return text;
		}

		public static T StyleAsSubtitle1<T>(this T text) where T : Text
		{
			text.StyleId = EnvironmentKeys.Text.Style.Subtitle1;
			return text;
		}

		public static T StyleAsSubtitle2<T>(this T text) where T : Text
		{
			text.StyleId = EnvironmentKeys.Text.Style.Subtitle2;
			return text;
		}

		public static T StyleAsBody1<T>(this T text) where T : Text
		{
			text.StyleId = EnvironmentKeys.Text.Style.Body1;
			return text;
		}

		public static T StyleAsBody2<T>(this T text) where T : Text
		{
			text.StyleId = EnvironmentKeys.Text.Style.Body2;
			return text;
		}

		public static T StyleAsCaption<T>(this T text) where T : Text
		{
			text.StyleId = EnvironmentKeys.Text.Style.Caption;
			return text;
		}

		public static T StyleAsOverline<T>(this T text) where T : Text
		{
			text.StyleId = EnvironmentKeys.Text.Style.Overline;
			return text;
		}

		public static T ApplyStyle<T>(this T view, Style style) where T : ContextualObject
		{
			style.Apply(view);
			return view;
		}

		private static T Apply<T>(T view, string styleId, ViewStyle style) where T : View
		{
			view.SetEnvironment(styleId, EnvironmentKeys.View.Border, style.Border);
			view.SetEnvironment(styleId, EnvironmentKeys.Colors.Background, style.BackgroundColor);
			view.SetEnvironment(styleId, EnvironmentKeys.View.Shadow, style.Shadow);
			view.SetEnvironment(styleId, EnvironmentKeys.View.ClipShape, style.Border);
			return view;
		}

		private static string GetStyleId<T>() where T : ViewStyle
		{
			var frame = new StackFrame(1);
			string className = frame.GetMethod().DeclaringType.Name;
			string styleId = $"{className}.{typeof(T)}";
			Console.WriteLine($"$Get style Id {styleId}");
			return styleId;
		}

		public static View Apply<T>(this View view) where T : ViewStyle, new()
		{
			string styleId = GetStyleId<T>();
			var result = Apply(view, styleId, new T());
			result.StyleId = styleId;
			return result;
		}

		public static Button Apply<T>(this Button button) where T : ButtonStyle, new()
		{
			string styleId = GetStyleId<T>();
			T style = new T();
			var result = Apply(button, styleId, style);
			result.SetEnvironment(styleId, EnvironmentKeys.Colors.Color, style.TextColor);
			result.SetEnvironment(styleId, EnvironmentKeys.Button.Padding, style.Padding);
			result.SetEnvironment(styleId, EnvironmentKeys.Fonts.Font, style.TextFont);
			result.StyleId = styleId;
			return result;
		}

		public static ProgressBar Apply<T>(this ProgressBar progressBar) where T : ProgressBarStyle, new()
		{
			string styleId = GetStyleId<T>();
			T style = new T();
			var result = Apply(progressBar, styleId, style);
			result.SetEnvironment(styleId, EnvironmentKeys.ProgressBar.ProgressColor, style.ProgressColor);
			result.StyleId = styleId;
			return result;
		}

		public static Slider Apply<T>(this Slider slider) where T : SliderStyle, new()
		{
			string styleId = GetStyleId<T>();
			T style = new T();
			var result = Apply(slider, styleId, style);
			result.SetEnvironment(styleId, EnvironmentKeys.Slider.TrackColor, style.TrackColor);
			result.SetEnvironment(styleId, EnvironmentKeys.Slider.ProgressColor, style.ProgressColor);
			result.SetEnvironment(styleId, EnvironmentKeys.Slider.ThumbColor, style.ThumbColor);
			result.StyleId = styleId;
			return result;
		}
	}
}
