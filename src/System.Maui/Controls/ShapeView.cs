using System;

namespace System.Maui
{
	/// <summary>
	/// A view that displays a shape.
	/// </summary>
	public class ShapeView : View
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
	}
}
