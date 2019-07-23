using System;

namespace HotUI
{
    public class ProgressBar : BoundControl<double>
    {
        public ProgressBar(
            Binding<double> value = null,
            bool isIndeterminate = false)
            : base(value, nameof(Value))
        {
            IsIndeterminate = isIndeterminate;
        }

        public ProgressBar(
            Func<double> value,
            bool isIndeterminate = false)
            : this((Binding<double>)value, isIndeterminate)
        {
        }

        bool _isIndeterminate;
        public bool IsIndeterminate
        {
            get => _isIndeterminate;
            set => SetValue(ref _isIndeterminate, value);
        }

        public double Value
        {
            get => BoundValue;
            private set => BoundValue = value;
        }
    }
}
