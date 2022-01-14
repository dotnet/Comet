using Comet.Layout;

using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Primitives;

// ReSharper disable once CheckNamespace
namespace Comet
{
	public static class LayoutExtensions
	{
		/// <summary>
		/// Set the padding to the default thickness.
		/// </summary>
		/// <param name="view"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Margin<T>(this T view) where T : View
		{
			var defaultThickness = new Thickness(10);
			view.Margin(defaultThickness);
			return view;
		}

		public static T Margin<T>(this T view, float? left = null, float? top = null, float? right = null, float? bottom = null) where T : View
		{
			view.Margin(new Thickness(
				left ?? 0,
				top ?? 0,
				right ?? 0,
				bottom ?? 0));
			return view;
		}

		public static T Margin<T>(this T view, float value) where T : View
		{
			view.Margin(new Thickness(value));
			return view;
		}

		public static T Overlay<T>(this T view, View overlayView) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.View.Overlay, overlayView);
			return view;
		}

		public static T Overlay<T>(this T view, Shape shape) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.View.Overlay, shape);
			return view;
		}

		public static Shape GetOverlay(this View view)
		{
			return view.GetEnvironment<Shape>(EnvironmentKeys.View.Overlay);
		}

		public static T Frame<T>(this T view, float? width = null, float? height = null, Alignment alignment = null) where T : View
		{
			view.FrameConstraints(new FrameConstraints(width, height, alignment));
			return view;
		}

		public static T Cell<T>(
			this T view,
			int row = 0,
			int column = 0,
			int rowSpan = 1,
			int colSpan = 1,
			float weightX = 1,
			float weightY = 1,
			float positionX = 0,
			float positionY = 0) where T : View
		{
			view.LayoutConstraints(new GridConstraints(row, column, rowSpan, colSpan, weightX, weightY, positionX, positionY));
			return view;
		}

		public static T NextRow<T>(this T view, int count = 1) where T : View
		{
			view.SetEnvironment(nameof(NextRow), count, false);
			return view;
		}
		public static T NextColumn<T>(this T view, int count = 1) where T : View
		{
			view.SetEnvironment(nameof(NextColumn), count, false);
			return view;
		}
		public static int GetIsNextColumn(this View view)
			=>view.GetEnvironment<int>(nameof(NextColumn), false);

		public static int GetIsNextRow(this View view)
			=> view.GetEnvironment<int>(nameof(NextRow), false);

		public static void SetFrameFromNativeView(
			this View view,
			Rectangle frame, LayoutAlignment defaultHorizontalAlignment = LayoutAlignment.Center, LayoutAlignment defaultVerticalAlignment = LayoutAlignment.Center)
		{
			if (view == null)
				return;
			var margin = view.GetMargin();
			if (!margin.IsEmpty)
			{
				frame.X += margin.Left;
				frame.Y += margin.Top;
				frame.Width -= margin.HorizontalThickness;
				frame.Height -= margin.VerticalThickness;
			}
			if (!view.MeasurementValid)
			{
				var sizeThatFits = view.Measure(frame.Size.Width, frame.Size.Height);
				view.MeasuredSize = sizeThatFits;
				view.MeasurementValid = true;
			}


			var width = view.MeasuredSize.Width - margin.HorizontalThickness;
			var height = view.MeasuredSize.Height - margin.VerticalThickness;

			var frameConstraints = view.GetFrameConstraints();

			
			var horizontalSizing = frameConstraints?.Alignment?.Horizontal ?? view.GetHorizontalLayoutAlignment(view.Parent as ContainerView,  defaultHorizontalAlignment);
			var verticalSizing = frameConstraints?.Alignment?.Vertical ?? view.GetVerticalLayoutAlignment(view.Parent as ContainerView, defaultVerticalAlignment);


			if (frameConstraints?.Width != null)
			{
				width = (float)frameConstraints.Width;
			}
			else
			{
				if (horizontalSizing == LayoutAlignment.Fill)
					width = frame.Width;
			}

			if (frameConstraints?.Height != null)
			{
				height = (float)frameConstraints.Height;
			}
			else
			{
				if (verticalSizing == LayoutAlignment.Fill)
					height = frame.Height;
			}

			var alignment = frameConstraints?.Alignment ?? Alignment.Center;

			var xFactor = .5f;
			switch (horizontalSizing)
			{
				case LayoutAlignment.Start:
					xFactor = 0;
					break;
				case LayoutAlignment.End:
					xFactor = 1;
					break;
			}

			var yFactor = .5f;
			switch (verticalSizing)
			{
				case LayoutAlignment.End:
					yFactor = 1;
					break;
				case LayoutAlignment.Start:
					yFactor = 0;
					break;
			}

			var x = frame.X + ((frame.Width - width) * xFactor);
			var y = frame.Y + ((frame.Height - height) * yFactor);
			view.Frame = new Rectangle(x, y, width, height);
		}

		public static T FillHorizontal<T>(this T view, bool cascades = false) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.HorizontalLayoutAlignment, LayoutAlignment.Fill, cascades);
			return view;
		}

		public static T FillVertical<T>(this T view, bool cascades = false) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.VerticalLayoutAlignment, LayoutAlignment.Fill, cascades);
			return view;
		}

		public static T FitHorizontal<T>(this T view, bool cascades = false) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.HorizontalLayoutAlignment, LayoutAlignment.Start, cascades);
			return view;
		}

		public static T FitVertical<T>(this T view, bool cascades = false) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.VerticalLayoutAlignment, LayoutAlignment.Start, cascades);
			return view;
		}

		public static LayoutAlignment GetHorizontalLayoutAlignment(this View view, ContainerView container, LayoutAlignment defaultSizing = LayoutAlignment.Start)
		{
			var sizing = view.GetEnvironment<LayoutAlignment?>(view, EnvironmentKeys.Layout.HorizontalLayoutAlignment);
			if (sizing != null) return (LayoutAlignment)sizing;

			if (container != null)
				sizing = view.GetEnvironment<LayoutAlignment?>(view, $"{container.GetType().Name}.{EnvironmentKeys.Layout.HorizontalLayoutAlignment}");
			return sizing ?? defaultSizing;
		}

		public static LayoutAlignment GetVerticalLayoutAlignment(this View view, ContainerView container, LayoutAlignment defaultSizing = LayoutAlignment.Start)
		{
			var sizing = view.GetEnvironment<LayoutAlignment?>(view, EnvironmentKeys.Layout.VerticalLayoutAlignment);
			if (sizing != null) return (LayoutAlignment)sizing;

			if (container != null)
				sizing = view.GetEnvironment<LayoutAlignment?>(view, $"{container.GetType().Name}.{EnvironmentKeys.Layout.VerticalLayoutAlignment}");
			return sizing ?? defaultSizing;
		}

		public static T VerticalLayoutAlignment<T>(this T view, LayoutAlignment alignment, bool cascades = false) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Layout.VerticalLayoutAlignment, alignment,cascades);

		public static T HorizontalLayoutAlignment<T>(this T view, LayoutAlignment alignment, bool cascades = false) where T : View
			=> view.SetEnvironment(EnvironmentKeys.Layout.HorizontalLayoutAlignment, alignment, cascades);


		public static T FrameConstraints<T>(this T view, FrameConstraints constraints, bool cascades = false) where T : View
		{

			view.SetEnvironment(EnvironmentKeys.Layout.VerticalLayoutAlignment, constraints?.Alignment?.Vertical, cascades);
			view.SetEnvironment(EnvironmentKeys.Layout.HorizontalLayoutAlignment, constraints?.Alignment?.Horizontal, cascades);
			view.SetEnvironment(EnvironmentKeys.Layout.FrameConstraints, constraints, cascades);
			return view;
		}

		public static FrameConstraints GetFrameConstraints(this View view, FrameConstraints defaultContraints = null)
		{
			var constraints = view.GetEnvironment<FrameConstraints>(view, EnvironmentKeys.Layout.FrameConstraints);
			return constraints ?? defaultContraints;
		}

		public static T Margin<T>(this T view, Thickness margin, bool cascades = false) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.Margin, margin, cascades);
			return view;
		}
		public static Thickness GetMargin(this View view, Thickness? defaultValue = null)
		{
			var margin = view.GetEnvironment<Thickness?>(view, EnvironmentKeys.Layout.Margin);
			return margin ?? defaultValue ?? Thickness.Zero;
		}

		public static T Padding<T>(this T view, Thickness padding, bool cascades = false) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.Padding, padding, cascades);
			return view;
		}

		public static Thickness GetPadding(this View view, Thickness? defaultValue = null)
		{
			var margin = view.GetEnvironment<Thickness?>(view, EnvironmentKeys.Layout.Padding);
			return margin ?? defaultValue ?? Thickness.Zero;
		}


		public static T LayoutConstraints<T>(this T view, object contraints, bool cascades = false) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.Constraints, contraints, cascades);
			return view;
		}

		public static object GetLayoutConstraints(this View view, object defaultValue = null)
		{
			var constraints = view.GetEnvironment<object>(view, EnvironmentKeys.Layout.Constraints);
			return constraints ?? defaultValue;
		}

		public static Size Measure(this View view, Size availableSize, bool includeMargin)
		{
			if (availableSize.Width <= 0 || availableSize.Height <= 0)
				return availableSize;
			
			if (includeMargin)
			{
				var margin = view.GetMargin();
				availableSize.Width -= margin.HorizontalThickness;
				availableSize.Height -= margin.VerticalThickness;

				var measuredSize = view.Measure(availableSize.Width,availableSize.Height);
				measuredSize.Width += margin.HorizontalThickness;
				measuredSize.Height += margin.VerticalThickness;
				return measuredSize;
			}

			return view.Measure(availableSize.Width, availableSize.Height);
		}

		public static T IgnoreSafeArea<T>(this T view) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.IgnoreSafeArea, true, false);
			return view;
		}

		public static bool GetIgnoreSafeArea(this View view, bool defaultValue) => (bool?)view.GetEnvironment(view, EnvironmentKeys.Layout.IgnoreSafeArea, false) ?? defaultValue;
	}
}
