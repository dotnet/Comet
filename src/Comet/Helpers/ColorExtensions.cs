using System;

// ReSharper disable once CheckNamespace
namespace Comet
{
	public static class ColorExtensions
	{

		public static Color WithAlpha(this Color color, float alpha)
			=> new Color(color.R, color.G, color.B, alpha);

		/// <summary>
		/// Set the color
		/// </summary>
		/// <param name="view"></param>
		/// <param name="color"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Color<T>(this T view, Color color) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Colors.Color, color, false);
			return view;
		}
		public static T Color<T>(this T view, Type type, Color color) where T : View
		{
			view.SetEnvironment(type, EnvironmentKeys.Colors.Color, color, true);
			return view;
		}

		public static Color GetColor<T>(this T view, Color defaultColor, ControlState state = ControlState.Default) where T : View
		{
			var color = view.GetEnvironment<Color>(EnvironmentKeys.Colors.Color,state);
			return color ?? defaultColor;
		}

		public static Color GetColor<T>(this T view, Type type, Color defaultColor) where T : View
		{
			var color = view.GetEnvironment<Color>(type, EnvironmentKeys.Colors.Color);
			return color ?? defaultColor;
		}

		/// <summary>
		/// Set the background color
		/// </summary>
		/// <param name="view"></param>
		/// <param name="color"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Background<T>(this T view, Color color, bool cascades = false) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Colors.BackgroundColor, color, cascades);
			return view;
		}

		/// <summary>
		/// Set the background color by hex value
		/// </summary>
		/// <param name="view"></param>
		/// <param name="color"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Background<T>(this T view, string color, bool cascades = false) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Colors.BackgroundColor, new Color(color), cascades);
			return view;
		}

		/// <summary>
		/// Set the background color
		/// </summary>
		/// <param name="view"></param>
		/// <param name="color"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Background<T>(this T view, Type type, Color color) where T : View
		{
			view.SetEnvironment(type, EnvironmentKeys.Colors.BackgroundColor, color, true);
			return view;
		}

		public static Color GetBackgroundColor(this View view, Type type, Color defaultColor = null, ControlState state = ControlState.Default)
		{
			var color = view?.GetEnvironment<Color>(type, EnvironmentKeys.Colors.BackgroundColor, state);
			return color ?? defaultColor;
		}

		public static Color GetBackgroundColor(this View view, Color defaultColor = null, ControlState state = ControlState.Default)
		{
			var color = view?.GetEnvironment<Color>(EnvironmentKeys.Colors.BackgroundColor,state);
			return color ?? defaultColor;
		}

		public static Color GetNavigationBackgroundColor(this View view, Color defaultColor = null)
		{
			var color = view.GetEnvironment<Color>(EnvironmentKeys.Navigation.BackgroundColor);
			return color ?? defaultColor;
		}
		public static Color GetNavigationTextColor(this View view, Color defaultColor = null)
		{
			var color = view.GetEnvironment<Color>(EnvironmentKeys.Navigation.TextColor);
			return color ?? defaultColor;
		}

		public static T TrackColor<T>(this T view, Color color, ControlState state = ControlState.Default) where T : Slider
		{
			view.SetEnvironment(EnvironmentKeys.Slider.TrackColor, color, cascades:false, state);
			return view;
		}

		public static Color GetTrackColor(this Slider view, Color defaultColor = null, ControlState state = ControlState.Default)
		{
			var color = view?.GetEnvironment<Color>(EnvironmentKeys.Slider.TrackColor, state);
			//Fall back to the default state before using the default color
			color ??= view?.GetEnvironment<Color>(EnvironmentKeys.Slider.TrackColor);
			return color ?? defaultColor;
		}

		public static T ProgressColor<T>(this T view, Color color, ControlState state = ControlState.Default) where T : Slider
		{
			view.SetEnvironment(EnvironmentKeys.Slider.ProgressColor, color, cascades:false, state);
			return view;
		}

		public static Color GetProgressColor(this Slider view, Color defaultColor = null, ControlState state = ControlState.Default)
		{
			var color = view?.GetEnvironment<Color>(EnvironmentKeys.Slider.ProgressColor, state);
			//Fall back to the default state before using the default color
			color ??= view?.GetEnvironment<Color>(EnvironmentKeys.Slider.ProgressColor);
			return color ?? defaultColor;
		}

		public static T ThumbColor<T>(this T view, Color color, ControlState state = ControlState.Default) where T : Slider
		{
			view.SetEnvironment(EnvironmentKeys.Slider.ThumbColor, color, cascades:false, state);
			return view;
		}

		public static Color GetThumbColor(this Slider view, Color defaultColor = null, ControlState state = ControlState.Default)
		{
			var color = view?.GetEnvironment<Color>(EnvironmentKeys.Slider.ThumbColor, state);
			//Fall back to the default state before using the default color
			color ??= view?.GetEnvironment<Color>(EnvironmentKeys.Slider.ThumbColor);
			return color ?? defaultColor;
		}


		public static T TrackColor<T>(this T view, Color color, ControlState state = ControlState.Default, bool cascades = false)
			where T : ProgressBar
		{
			view.SetEnvironment(EnvironmentKeys.ProgressBar.TrackColor, color, cascades, state);
			return view;
		}

		public static Color GetTrackColor(this ProgressBar view, Color defaultColor = null, ControlState state = ControlState.Default)
		{
			var color = view?.GetEnvironment<Color>(EnvironmentKeys.ProgressBar.TrackColor, state);
			//Fall back to the default state before using the default color
			color ??= view?.GetEnvironment<Color>(EnvironmentKeys.ProgressBar.TrackColor);
			return color ?? defaultColor;
		}

		public static T ProgressColor<T>(this T view, Color color, bool cascades = false) where T : ProgressBar
		{
			view.SetEnvironment(EnvironmentKeys.ProgressBar.ProgressColor, color, cascades);
			return view;
		}

		public static Color GetProgressColor(this ProgressBar view, Color defaultColor = null, ControlState state = ControlState.Default)
		{
			var color = view?.GetEnvironment<Color>(EnvironmentKeys.ProgressBar.ProgressColor, state);
			//Fall back to the default state before using the default color
			color ??= view?.GetEnvironment<Color>(EnvironmentKeys.ProgressBar.ProgressColor);
			return color ?? defaultColor;
		}

	}
}
