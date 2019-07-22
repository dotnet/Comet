using System;

namespace HotUI
{
    /// <summary>
    /// A view that displays a shape.
    /// </summary>
    public class ShapeView : BoundControl<Shape>
    {
        public ShapeView(Binding<Shape> value) : base(value, nameof(Shape))
        {
        }

        public ShapeView(Func<Shape> valueBuilder) : this((Binding<Shape>)valueBuilder)
        {
        }
        
        public Shape Shape
        {
            get => BoundValue;
            private set => BoundValue = value;
        }
    }
}