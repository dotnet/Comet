using System;

namespace HotUI
{
    public class Toggle : View
    {

        public Toggle(bool isOn)
        {
            IsOn = isOn;
        }

        public Toggle(Func<bool> builder)
        {
            IsOnBinding = builder;
        }
      
       
        private bool isOn;
        public bool IsOn
        {
            get => isOn;
            set => this.SetValue(State, ref isOn, value, ViewPropertyChanged);
        }

        public Func<bool> IsOnBinding { get; private set; }

        protected override void WillUpdateView()
        {
            base.WillUpdateView();
            if (IsOnBinding != null)
            {
                State.StartProperty();
                var on = IsOnBinding.Invoke();
                var props = State.EndProperty();
                var propCount = props.Length;
                if (propCount > 0)
                {
                    State.BindingState.AddViewProperty(props, (s, o) => IsOn = IsOnBinding.Invoke());
                }
                IsOn = on;
            }
        }

        public Action<bool> IsOnChanged { get; set; }
    }
}
