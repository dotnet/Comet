using System;
using AToggle = Android.Widget.ToggleButton;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class ToggleHandler : AToggle, IView
    {
        public ToggleHandler() : base(AndroidContext.CurrentContext)
        {
            Click += HandleClick;
        }

        private void HandleClick(object sender, EventArgs e) => toggle?.IsOnChanged?.Invoke(this.Checked);

        public AView View => this;
        public object NativeView => View;
        public bool HasContainer { get; set; } = false;

        Toggle toggle;

        public void Remove(View view)
        {
            toggle = null;
        }

        public void SetView(View view)
        {
            toggle = view as Toggle;
            this.UpdateProperties(toggle);
        }

        public void UpdateValue(string property, object value)
        {
            this.UpdateProperty(property, value);
        }
    }

    public static partial class ControlExtensions
    {
        public static void UpdateProperties(this AToggle view, Toggle hView)
        {
            view.Checked = hView?.IsOn ?? false;
            view.UpdateBaseProperties(hView);
        }

        public static bool UpdateProperty(this AToggle view, string property, object value)
        {
            switch (property)
            {
                case nameof(Toggle.IsOn):
                    view.Checked = (bool)value;
                    return true;
            }

            return view.UpdateBaseProperty(property, value);
        }
    }
}