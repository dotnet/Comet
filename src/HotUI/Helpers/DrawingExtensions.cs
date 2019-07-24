using System;
using HotUI.Graphics;

// ReSharper disable once CheckNamespace
namespace HotUI
{
    public static class DrawingExtensions
    {
        public static T Shadow<T>(this T view, Color color = null, float? radius = null, float? x = null, float? y = null, Type type = null) where T : View
        {
            var shadow = view.GetShadow() ?? new Shadow();

            if (color != null)
                shadow = shadow.WithColor(color);

            if (radius != null)
                shadow = shadow.WithRadius((float) radius);

            if (x != null || y != null)
            {
                var newX = x ?? shadow.Offset.Width;
                var newY = y ?? shadow.Offset.Height;
                shadow = shadow.WithOffset(new SizeF(newX, newY));
            }
            view.SetEnvironment(type,EnvironmentKeys.View.Shadow, shadow, type != null);
            return view;
        }

        public static Shadow GetShadow(this View view, Shadow defaultShadow = null, Type type = null)
        {
            var shadow = view.GetEnvironment<Shadow>(type,EnvironmentKeys.View.Shadow);
            return shadow ?? defaultShadow;
        }
        public static T ClipShape<T>(this T view, Shape shape, Type type = null) where T : View
        {
            view.SetEnvironment(type,EnvironmentKeys.View.ClipShape, shape, type != null);
            return view;
        }

        public static Shape GetClipShape(this View view, Shape defaultShape = null, Type type = null)
        {
            var shape = view.GetEnvironment<Shape>(type,EnvironmentKeys.View.ClipShape);
            return shape ?? defaultShape;
        }

        public static T Stroke<T>(this T shape, Color color, float lineWidth, bool cascades = true, Type type = null) where T : Shape
        {
            if (type != null && !cascades)
                Logger.Fatal($"Setting a type, and cascades = false does nothing!");
            shape.SetEnvironment(type, EnvironmentKeys.Shape.LineWidth, lineWidth, cascades);
            shape.SetEnvironment(type, EnvironmentKeys.Shape.StrokeColor, color, cascades);
            return shape;
        }
        
        public static T Fill<T>(this T shape, Color color, bool cascades = true, Type type = null) where T : Shape
        {
            if (type != null && !cascades)
                Logger.Fatal($"Setting a type, and cascades = false does nothing!");
            shape.SetEnvironment(type, EnvironmentKeys.Shape.Fill, color,cascades);
            return shape;
        }
        
        public static T Fill<T>(this T shape, Gradient gradient, bool cascades = true, Type type = null) where T : Shape
        {
            if (type != null && !cascades)
                Logger.Fatal($"Setting a type, and cascades = false does nothing!");
            shape.SetEnvironment(type, EnvironmentKeys.Shape.Fill, gradient,cascades);
            return shape;
        }
        
        public static T Style<T>(this T shape, DrawingStyle drawingStyle, bool cascades = true, Type type = null) where T : Shape
        {
            if (type != null && !cascades)
                Logger.Fatal($"Setting a type, and cascades = false does nothing!");
            shape.SetEnvironment(type, EnvironmentKeys.Shape.DrawingStyle, drawingStyle, cascades);
            return shape;
        }

        public static float GetLineWidth(this Shape shape,View view, float defaultStroke, Type type = null)
        {
            var stroke = shape.GetEnvironment<float?>(view, type, EnvironmentKeys.Shape.LineWidth);
            return stroke ?? defaultStroke;
        }

        public static Color GetStrokeColor(this Shape shape,View view, Color defaultColor, Type type = null)
        {
            var color = shape.GetEnvironment<Color>(view, type, EnvironmentKeys.Shape.StrokeColor);
            return color ?? defaultColor;
        }
        
        public static object GetFill(this Shape shape,View view, object defaultFill = null, Type type = null)
        {
            var fill = shape.GetEnvironment<object>(view, type, EnvironmentKeys.Shape.Fill);
            return fill ?? defaultFill;
        }
        
        public static DrawingStyle GetDrawingStyle(this Shape shape,View view, DrawingStyle defaultDrawingStyle = DrawingStyle.StrokeFill, Type type = null)
        {
            var drawingStyle = shape.GetEnvironment<DrawingStyle?>(view, type, EnvironmentKeys.Shape.DrawingStyle);
            return drawingStyle ?? defaultDrawingStyle;
        }
    }
}