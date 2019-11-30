using System;
using System.Collections.Generic;
using System.Text;

namespace Comet
{
    public class Stepper : View
    {
        public Stepper(Binding<double> value = null)
        {
            Value = value;
        }

        Binding<double> _value;
        public Binding<double> Value
        {
            get => _value;
            private set => this.SetBindingValue(ref _value, value);
        }
    }
}
