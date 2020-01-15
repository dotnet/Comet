using Comet.Layout;
using System.Drawing;

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

		public static void SetFrameFromNativeView(
			this View view,
			RectangleF frame)
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

			var sizeThatFits = view.Measure(frame.Size);
			view.MeasuredSize = sizeThatFits;
			view.MeasurementValid = true;

			var width = sizeThatFits.Width;
			var height = sizeThatFits.Height;

			var frameConstraints = view.GetFrameConstraints();

			if (frameConstraints?.Width != null)
			{
				width = (float)frameConstraints.Width;
			}
			else
			{
				var horizontalSizing = view.GetHorizontalSizing(view.Parent as ContainerView, Sizing.Fit);
				if (horizontalSizing == Sizing.Fill)
					width = frame.Width;
			}

			if (frameConstraints?.Height != null)
			{
				height = (float)frameConstraints.Height;
			}
			else
			{
				var verticalSizing = view.GetVerticalSizing(view.Parent as ContainerView, Sizing.Fit);
				if (verticalSizing == Sizing.Fill)
					height = frame.Height;
			}

			var alignment = frameConstraints?.Alignment ?? Alignment.Center;

			var xFactor = .5f;
			switch (alignment.Horizontal)
			{
				case HorizontalAlignment.Leading:
					xFactor = 0;
					break;
				case HorizontalAlignment.Trailing:
					xFactor = 1;
					break;
			}

			var yFactor = .5f;
			switch (alignment.Vertical)
			{
				case VerticalAlignment.Bottom:
					yFactor = 1;
					break;
				case VerticalAlignment.Top:
					yFactor = 0;
					break;
			}

			var x = frame.X + ((frame.Width - width) * xFactor);
			var y = frame.Y + ((frame.Height - height) * yFactor);
			view.Frame = new RectangleF((float)x, (float)y, width, height);
			view.RequestLayout();
		}

		public static T FillHorizontal<T>(this T view, bool cascades = true) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.HorizontalSizing, Sizing.Fill, cascades);
			return view;
		}

		public static T FillVertical<T>(this T view, bool cascades = true) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.VerticalSizing, Sizing.Fill, cascades);
			return view;
		}

		public static T FitHorizontal<T>(this T view, bool cascades = true) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.HorizontalSizing, Sizing.Fit, cascades);
			return view;
		}

		public static T FitVertical<T>(this T view, bool cascades = true) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.VerticalSizing, Sizing.Fit, cascades);
			return view;
		}

		public static Sizing GetHorizontalSizing(this View view, ContainerView container, Sizing defaultSizing = Sizing.Fit)
		{
			var sizing = view.GetEnvironment<Sizing?>(view, EnvironmentKeys.Layout.HorizontalSizing);
			if (sizing != null) return (Sizing)sizing;

			if (container != null)
				sizing = view.GetEnvironment<Sizing?>(view, $"{container.GetType().Name}.{EnvironmentKeys.Layout.HorizontalSizing}");
			return sizing ?? defaultSizing;
		}

		public static Sizing GetVerticalSizing(this View view, ContainerView container, Sizing defaultSizing = Sizing.Fit)
		{
			var sizing = view.GetEnvironment<Sizing?>(view, EnvironmentKeys.Layout.VerticalSizing);
			if (sizing != null) return (Sizing)sizing;

			if (container != null)
				sizing = view.GetEnvironment<Sizing?>(view, $"{container.GetType().Name}.{EnvironmentKeys.Layout.VerticalSizing}");
			return sizing ?? defaultSizing;
		}

		public static T FrameConstraints<T>(this T view, FrameConstraints constraints, bool cascades = false) where T : View
		{
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
			return margin ?? defaultValue ?? Thickness.Empty;
		}

		public static T Padding<T>(this T view, Thickness padding, bool cascades = false) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.Padding, padding, cascades);
			return view;
		}

		public static Thickness GetPadding(this View view, Thickness? defaultValue = null)
		{
			var margin = view.GetEnvironment<Thickness?>(view, EnvironmentKeys.Layout.Padding);
			return margin ?? defaultValue ?? Thickness.Empty;
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

		public static SizeF Measure(this View view, SizeF availableSize, bool includeMargin)
		{
			if (availableSize.Width <= 0 || availableSize.Height <= 0)
				return availableSize;
			
			if (includeMargin)
			{
				var margin = view.GetMargin();
				availableSize.Width -= margin.HorizontalThickness;
				availableSize.Height -= margin.VerticalThickness;

				var measuredSize = view.Measure(availableSize);
				measuredSize.Width += margin.HorizontalThickness;
				measuredSize.Height += margin.VerticalThickness;
				return measuredSize;
			}

			return view.Measure(availableSize);
		}

		public static T IgnoreSafeArea<T>(this T view) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Layout.IgnoreSafeArea, true, false);
			return view;
		}

		public static bool GetIgnoreSafeArea(this View view, bool defaultValue) => (bool?)view.GetEnvironment(view, EnvironmentKeys.Layout.IgnoreSafeArea, false) ?? defaultValue;
	}
}
