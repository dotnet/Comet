using System;
using System.Collections.Generic;
using FToggle = Xamarin.Forms.Switch;
using FView = Xamarin.Forms.View;

namespace HotUI.Forms
{
    public class ToggleHandler : FToggle, IFormsView
    {
		public static readonly PropertyMapper<Toggle, FView, ToggleHandler> Mapper = new PropertyMapper<Toggle, FView, ToggleHandler> ()
		{ 
            [nameof(Toggle.IsOn)] = MapIsOnProperty
        };

        public ToggleHandler()
        {
            this.Toggled += HandleToggled;
        }

        void HandleToggled(object sender, EventArgs e) => toggle?.IsOnChanged?.Invoke(IsToggled);

        public FView View => this;

        Toggle toggle;

        public void Remove(View view)
        {
            toggle = null;
        }

        public void SetView(View view)
        {
            toggle = view as Toggle;
            Mapper.UpdateProperties(this, toggle);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, toggle, property);
        }

        public static bool MapIsOnProperty(ToggleHandler nativeView, Toggle virtualView)
        {
            Console.WriteLine($"Xamarin.Forms Switch should be: {virtualView.IsOn}");
            nativeView.IsToggled = virtualView.IsOn;
            return true;
        }
    }
}