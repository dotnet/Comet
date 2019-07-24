using System;
using HotUI.Graphics;

// ReSharper disable once CheckNamespace
namespace HotUI
{
    public static class DrawingExtensions
    {
        public static T Shadow<T>(this T view, Color color = null, float? radius = null, float? x = null, float? y = null) where T : View
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

            view.SetEnvironment(EnvironmentKeys.View.Shadow, shadow);
            return view;
        }

        public static Shadow GetShadow(this View view, Shadow defaultShadow = null)
        {
            var shadow = view.GetEnvironment<Shadow>(EnvironmentKeys.View.Shadow);
            return shadow ?? defaultShadow;
        }

        public static T ClipShape<T>(this T view, Shape shape) where T : View
        {
            view.SetEnvironment(EnvironmentKeys.View.ClipShape, shape, false);
            return view;
        }

        public static Shape GetClipShape(this View view, Shape defaultShape = null)
        {
            var shape = view.GetEnvironment<Shape>(EnvironmentKeys.View.ClipShape);
            return shape ?? defaultShape;
        }

        public static T Stroke<T>(this T shape, Color color, float lineWidth, bool cascades = true) where T : Shape
        {
            shape.SetEnvironment(EnvironmentKeys.Shape.LineWidth, lineWidth, cascades);
            shape.SetEnvironment(EnvironmentKeys.Shape.StrokeColor, color, cascades);
            return shape;
        }
        
        public static T Fill<T>(this T shape, Color color, bool cascades = true) where T : Shape
        {
            shape.SetEnvironment(EnvironmentKeys.Shape.Fill, color,cascades);
            return shape;
        }
        
        public static T Fill<T>(this T shape, Gradient gradient, bool cascades = true) where T : Shape
        {
            shape.SetEnvironment(EnvironmentKeys.Shape.Fill, gradient,cascades);
            return shape;
        }
        
        public static T Style<T>(this T shape, DrawingStyle drawingStyle, bool cascades = true) where T : Shape
        {
            shape.SetEnvironment(EnvironmentKeys.Shape.DrawingStyle, drawingStyle,cascades);
            return shape;
        }

        public static float GetLineWidth(this Shape shape,View view, float defaultStroke)
        {
            var stroke = shape.GetEnvironment<float?>(view,EnvironmentKeys.Shape.LineWidth);
            return stroke ?? defaultStroke;
        }

        public static Color GetStrokeColor(this Shape shape,View view, Color defaultColor)
        {
            var color = shape.GetEnvironment<Color>(view, EnvironmentKeys.Shape.StrokeColor);
            return color ?? defaultColor;
        }
        
        public static object GetFill(this Shape shape,View view, object defaultFill = null)
        {
            var fill = shape.GetEnvironment<object>(view, EnvironmentKeys.Shape.Fill);
            return fill ?? defaultFill;
        }
        
        public static DrawingStyle GetDrawingStyle(this Shape shape,View view, DrawingStyle defaultDrawingStyle = DrawingStyle.StrokeFill)
        {
            var drawingStyle = shape.GetEnvironment<DrawingStyle?>(view, EnvironmentKeys.Shape.DrawingStyle);
            return drawingStyle ?? defaultDrawingStyle;
        }
    }
}