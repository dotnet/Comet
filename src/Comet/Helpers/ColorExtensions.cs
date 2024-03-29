﻿using System;
using Comet.Graphics;
using Microsoft.Maui.Graphics;


// ReSharper disable once CheckNamespace
namespace Comet
{
	public static class ColorExtensions
	{

		public static Color WithAlpha(this Color color, float alpha)
			=> new Color(color.Red, color.Green, color.Blue, alpha);

		/// <summary>
		/// Set the color
		/// </summary>
		/// <param name="view"></param>
		/// <param name="color"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Color<T>(this T view, Binding<Color> color) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Colors.Color, color, false);
			return view;
		}
		public static T Color<T>(this T view, Func<Color> color) where T : View => view.Color((Binding<Color>)color);

		public static T Color<T>(this T view, Type type, Binding<Color> color) where T : View
		{
			view.SetEnvironment(type, EnvironmentKeys.Colors.Color, color, true);
			return view;
		}
		public static T Color<T>(this T view, Type type, Func<Color> color) where T : View => view.Color(type: type, (Binding<Color>)color);

		public static Color GetColor<T>(this T view, Color defaultColor = null, ControlState state = ControlState.Default) where T : View
		{
			var color = view.GetEnvironment<Color>(EnvironmentKeys.Colors.Color, state);
			return color ?? defaultColor;
		}

		public static Color GetColor<T>(this T view, Type type, Color defaultColor = null) where T : View
		{
			var color = view.GetEnvironment<Color>(type, EnvironmentKeys.Colors.Color);
			return color ?? defaultColor;
		}

		public static Paint GetPaintColor<T>(this T view, Paint defaultColor = null, ControlState state = ControlState.Default) where T : View
		{
			var color = view.GetEnvironment<Paint>(EnvironmentKeys.Colors.Color, state);
			return color ?? defaultColor;
		}

		public static Paint GetPaintColor<T>(this T view, Type type, Paint defaultColor = null) where T : View
		{
			var color = view.GetEnvironment<Paint>(type, EnvironmentKeys.Colors.Color);
			return color ?? defaultColor;
		}

		/// <summary>
		/// Set the background color
		/// </summary>
		/// <param name="view"></param>
		/// <param name="color"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Background<T>(this T view, Binding<Color> color, bool cascades = false) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Colors.Background, color, cascades);
			return view;
		}
		public static T Background<T>(this T view, Func<Color> color, bool cascades = false) where T : View => view.Background((Binding<Color>)color, cascades);

		/// <summary>
		/// Set the background color by hex value
		/// </summary>
		/// <param name="view"></param>
		/// <param name="colorHex"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Background<T>(this T view, Binding<string> colorHex, bool cascades = false) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Colors.Background, colorHex, cascades);
			return view;
		}
		public static T Background<T>(this T view, Func<string> colorHex, bool cascades = false) where T : View => view.Background((Binding<string>)colorHex, cascades);

		/// <summary>
		/// Set the background color
		/// </summary>
		/// <param name="view"></param>
		/// <param name="color"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Background<T>(this T view, Type type, Binding<Color> color) where T : View
		{
			view.SetEnvironment(type, EnvironmentKeys.Colors.Background, color, true);
			return view;
		}
		public static T Background<T>(this T view, Type type, Func<Color> color) where T : View => view.Background(type, (Binding<Color>)color);
		public static T Background<T>(this T view, Type type, Binding<Paint> paint) where T : View
		{
			view.SetEnvironment(type, EnvironmentKeys.Colors.Background, paint, true);
			return view;
		}
		public static T Background<T>(this T view, Type type, Func<Paint> paint) where T : View => view.Background(type, (Binding<Paint>)paint);

		public static Paint GetBackground(this View view, Type type, Paint defaultColor = null, ControlState state = ControlState.Default)
		{
			var color = view?.GetEnvironment<object>(type, EnvironmentKeys.Colors.Background, state);
			return color.ConvertToPaint() ?? defaultColor;
		}

		public static Paint GetBackground(this View view, Paint defaultColor = null, ControlState state = ControlState.Default)
		{
			var color = view?.GetEnvironment<object>(EnvironmentKeys.Colors.Background, state);
			return color.ConvertToPaint() ?? defaultColor;
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


		public static T Opacity<T>(this T view, Binding<double> opacity, bool cascades = false) where T : View 
			=> view.SetEnvironment(EnvironmentKeys.View.Opacity, opacity, cascades);
		public static T Opacity<T>(this T view, Func<double> opacity, bool cascades = false) where T : View 
			=> view.Opacity((Binding<double>)opacity,cascades);
		
		public static double GetOpacity(this View view, ControlState state = ControlState.Default)
		{
			var opacity = view?.GetEnvironment<double?>(EnvironmentKeys.View.Opacity, state);
			//Fall back to the default state before using the default color
			opacity ??= view?.GetEnvironment<double?>(EnvironmentKeys.View.Opacity);
			return opacity ?? 1;
		}


	}
}
