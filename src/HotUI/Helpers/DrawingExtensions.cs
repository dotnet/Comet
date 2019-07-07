using System;
namespace HotUI 
{
	public static class DrawingExtensions 
	{

		public static T ClipShape<T> (this T view, Shape shape) where T : View
		{
			view.SetEnvironment (EnvironmentKeys.View.ClipShape, shape);
			return view;
		}
		
		public static Shape GetShape (this View view, Shape defaultShape = null)
		{
			var shape = view.GetEnvironment<Shape> (EnvironmentKeys.View.ClipShape);
			return shape ?? defaultShape;
		}
		
		public static T Stroke<T> (this T shape, Color color, float lineWidth) where T : Shape
		{
			//shape.SetEnvironment (EnvironmentKeys.View.ClipShape, shape);
			return shape;
		}
		
		public static float GetStroke (this Shape view, float defaultStroke)
		{
			//var stroke = view.GetEnvironment<float?> (EnvironmentKeys.View.ClipShape);
			return defaultStroke;
		}
    }
}
