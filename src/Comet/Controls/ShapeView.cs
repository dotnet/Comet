using System;
using Comet.Graphics;
using Microsoft.Maui.Graphics;

namespace Comet
{
	/// <summary>
	/// A view that displays a shape.
	/// </summary>
	public class ShapeView : View, IDrawable
	{
		public ShapeView(Binding<Shape> value)
		{
			Shape = value;
		}
		public ShapeView(Func<Shape> value)
		{
			Shape = value;
		}

		Binding<Shape> _shape;
		public Binding<Shape> Shape
		{
			get => _shape;
			private set => this.SetBindingValue(ref _shape, value);
		}

		void IDrawable.Draw(ICanvas canvas, RectangleF dirtyRect) {
			var backgroundColor = this.GetBackgroundColor(Colors.White);
			//canvas.Clear(backgroundColor);
			//TODO: Remove this later, it's a temp hack for Android
			if (canvas.DisplayScale > 1)
			{
				dirtyRect.Width /= canvas.DisplayScale;
				dirtyRect.Height /= canvas.DisplayScale;
			}
			var padding = this.GetPadding();
			dirtyRect = dirtyRect.ApplyPadding(padding);

			var shape = Shape.CurrentValue;
			var drawingStyle = shape.GetDrawingStyle(this, DrawingStyle.StrokeFill);
			var strokeColor = shape.GetStrokeColor(this, Colors.Black);
			var strokeWidth = shape.GetLineWidth(this, 1);
			var fill = shape.GetFill(this);
			canvas.DrawShape(shape, dirtyRect, drawingStyle, strokeWidth, strokeColor, fill);
		}
	}
}
