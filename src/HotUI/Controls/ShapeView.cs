using System;

namespace HotUI
{
    /// <summary>
    /// A view that displays a shape.
    /// </summary>
    public class ShapeView : Control
    {
        public ShapeView()
        {
        }

        public ShapeView(Shape value) : base(true)
        {
            Shape = value;
        }

        public ShapeView(Func<Shape> valueBuilder)
        {
            ShapeBinding = valueBuilder;
        }

        private Shape _value;

        public Shape Shape
        {
            get => _value;
            private set => this.SetValue(State, ref _value, value, ViewPropertyChanged);
        }

        public Func<Shape> ShapeBinding { get; }

        protected override void WillUpdateView()
        {
            base.WillUpdateView();
            if (ShapeBinding != null)
            {
                State.StartProperty();
                var shape = ShapeBinding.Invoke();
                var props = State.EndProperty();
                var propCount = props.Length;
                if (propCount > 0)
                {
                    State.BindingState.AddViewProperty(props, (s, o) => Shape = ShapeBinding.Invoke());
                }

                Shape = shape;
            }
        }
    }
}