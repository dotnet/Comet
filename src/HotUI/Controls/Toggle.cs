using System;

namespace HotUI
{
    public class Toggle : BoundControl<bool>
    {
        public Toggle (
            Binding<bool> value = null,
            Action<bool> onChanged = null) : base(value, nameof(IsOn))
        {
            IsOnChanged = new MulticastAction<bool>(value, onChanged);
        }
        
        public Toggle (
            Func<bool> value = null,
            Action<bool> onChanged = null) : this((Binding<bool>)value, onChanged)
        {
        }
        
        public bool IsOn
        {
            get => BoundValue;
            set => BoundValue = value;
        }
        
        public Action<bool> IsOnChanged { get; private set; }
    }
}
