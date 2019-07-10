using System;
using HotUI.Drawing;

namespace HotUI 
{
	public static class DrawingExtensions 
	{
		public static T Shadow<T> (this T view, Color color = null, float? radius = null, float? x = null, float? y = null) where T : View
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

			view.SetEnvironment (EnvironmentKeys.View.Shadow, shadow);
			return view;
		}
		
		public static Shadow GetShadow (this View view, Shadow defaultShadow = null)
		{
			var shadow = view.GetEnvironment<Shadow> (EnvironmentKeys.View.Shadow);
			return shadow ?? defaultShadow;
		}
		
		public static T ClipShape<T> (this T view, Shape shape) where T : View
		{
			view.SetEnvironment (EnvironmentKeys.View.ClipShape, shape);
			return view;
		}
		
		public static Shape GetClipShape (this View view, Shape defaultShape = null)
		{
			var shape = view.GetEnvironment<Shape> (EnvironmentKeys.View.ClipShape);
			return shape ?? defaultShape;
		}
		
		public static T Stroke<T> (this T shape, Color color, float lineWidth) where T : Shape
		{
            shape.SetEnvironment(EnvironmentKeys.Shape.LineWidth, lineWidth);
            shape.SetEnvironment(EnvironmentKeys.Shape.Color, color);
            return shape;
		}
		
		public static float GetStroke (this Shape shape, float defaultStroke)
		{
			var stroke = shape.GetEnvironment<float?> (EnvironmentKeys.Shape.LineWidth);
			return stroke ?? defaultStroke;
		}

        public static Color GetColor(this Shape shape, Color defaultColor)
        {
            var color = shape.GetEnvironment<Color>(EnvironmentKeys.Shape.Color);
            return color ?? defaultColor;
        }
    }
}
