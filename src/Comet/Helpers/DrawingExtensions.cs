using System;

using Comet.Graphics;
using Microsoft.Maui.Graphics;

// ReSharper disable once CheckNamespace
namespace Comet
{
	public static class DrawingExtensions
	{
		public static T Shadow<T>(this T view, Color color = null, float? radius = null, float? x = null, float? y = null, Type type = null) where T : View
		{
			var shadow = view.GetShadow() ?? new Shadow();

			if (color != null)
				shadow = shadow.WithColor(color);

			if (radius != null)
				shadow = shadow.WithRadius((float)radius);

			if (x != null || y != null)
			{
				var newX = x ?? shadow.Offset.Width;
				var newY = y ?? shadow.Offset.Height;
				shadow = shadow.WithOffset(new Size(newX, newY));
			}
			if (type != null)
				view.SetEnvironment(type, EnvironmentKeys.View.Shadow, shadow, true);
			else
				view.SetEnvironment(EnvironmentKeys.View.Shadow, shadow, false);
			return view;
		}

		public static Shadow GetShadow(this View view, Shadow defaultShadow = null, Type type = null)
		{
			var shadow = view.GetEnvironment<Shadow>(type, EnvironmentKeys.View.Shadow);
			return shadow ?? defaultShadow;
		}
		public static T ClipShape<T>(this T view, Shape shape, Type type = null) where T : View
		{
			if (type != null)
				view.SetEnvironment(type, EnvironmentKeys.View.ClipShape, shape, true);
			else
				view.SetEnvironment(EnvironmentKeys.View.ClipShape, shape, false);
			return view;
		}

		public static Shape GetClipShape(this View view, Shape defaultShape = null, Type type = null)
		{
			var shape = view.GetEnvironment<Shape>(type, EnvironmentKeys.View.ClipShape);
			return shape ?? defaultShape;
		}

		public static T Stroke<T>(this T shape, Color color, float lineWidth, bool cascades = true, Type type = null) where T : Shape
		{
			if (type != null && !cascades)
				Logger.Fatal($"Setting a type, and cascades = false does nothing!");
			if (type != null)
			{
				shape.SetEnvironment(type, EnvironmentKeys.Shape.LineWidth, lineWidth, true);
				shape.SetEnvironment(type, EnvironmentKeys.Shape.StrokeColor, color, true);
			}
			else
			{
				shape.SetEnvironment(EnvironmentKeys.Shape.LineWidth, lineWidth, cascades);
				shape.SetEnvironment(EnvironmentKeys.Shape.StrokeColor, color, cascades);
			}
			return shape;
		}

		public static T Fill<T>(this T shape, Color color, bool cascades = true, Type type = null) where T : Shape
		{
			if (type != null && !cascades)
				Logger.Fatal($"Setting a type, and cascades = false does nothing!");
			if (type != null)
				shape.SetEnvironment(type, EnvironmentKeys.Shape.Fill, color, true);
			else
				shape.SetEnvironment(EnvironmentKeys.Shape.Fill, color, cascades);

			return shape;
		}

		public static T Fill<T>(this T shape, Gradient gradient, bool cascades = true, Type type = null) where T : Shape
		{
			if (type != null && !cascades)
				Logger.Fatal($"Setting a type, and cascades = false does nothing!");
			if (type != null)
				shape.SetEnvironment(type, EnvironmentKeys.Shape.Fill, gradient, true);
			else
				shape.SetEnvironment(EnvironmentKeys.Shape.Fill, gradient, cascades);
			return shape;
		}

		public static T Style<T>(this T shape, DrawingStyle drawingStyle, bool cascades = true, Type type = null) where T : Shape
		{
			if (type != null && !cascades)
				Logger.Fatal($"Setting a type, and cascades = false does nothing!");
			if (type != null)
				shape.SetEnvironment(type, EnvironmentKeys.Shape.DrawingStyle, drawingStyle, true);
			else
				shape.SetEnvironment(EnvironmentKeys.Shape.DrawingStyle, drawingStyle, cascades);
			return shape;
		}

		public static float GetLineWidth(this Shape shape, View view, float defaultStroke, Type type = null)
		{
			var stroke = shape.GetEnvironment<float?>(view, type, EnvironmentKeys.Shape.LineWidth);
			return stroke ?? defaultStroke;
		}

		public static Color GetStrokeColor(this Shape shape, View view, Color defaultColor, Type type = null)
		{
			var color = shape.GetEnvironment<Color>(view, type, EnvironmentKeys.Shape.StrokeColor);
			return color ?? defaultColor;
		}

		public static object GetFill(this Shape shape, View view, object defaultFill = null, Type type = null)
		{
			var fill = shape.GetEnvironment<object>(view, type, EnvironmentKeys.Shape.Fill);
			return fill ?? defaultFill;
		}

		public static DrawingStyle GetDrawingStyle(this Shape shape, View view, DrawingStyle defaultDrawingStyle = DrawingStyle.StrokeFill, Type type = null)
		{
			var drawingStyle = shape.GetEnvironment<DrawingStyle?>(view, type, EnvironmentKeys.Shape.DrawingStyle);
			return drawingStyle ?? defaultDrawingStyle;
		}

		public static T Border<T>(this T view, Shape shape, Type type = null) where T : View
		{
			view.SetEnvironment(type, EnvironmentKeys.View.Border, shape, type != null);
			return view;
		}

		public static Shape GetBorder(this View view, Shape defaultShape = null, Type type = null)
		{
			var shape = view.GetEnvironment<Shape>(type, EnvironmentKeys.View.Border);
			return shape ?? defaultShape;
		}

		public static T RoundedBorder<T>(this T view, float radius = 4, Color color = null, float strokeSize = 1, bool filled = false, Type type = null) where T : View
		{
			var finalColor = color ?? Colors.Black;
			var shape = new RoundedRectangle(radius).Stroke(finalColor, strokeSize);
			view.ClipShape(shape);
			view.Border(shape);
			if (filled)
				view.Background(color);
			return view;
		}
	}
}
