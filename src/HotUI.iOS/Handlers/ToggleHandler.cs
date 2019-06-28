using System;
using System.Collections.Generic;
using UIKit;
namespace HotUI.iOS
{
    public class ToggleHandler : UISwitch, IUIView
    {
		private static readonly PropertyMapper<Toggle, UISwitch> Mapper = new PropertyMapper<Toggle, UISwitch> ()
		{ 
            [nameof(Toggle.IsOn)] = MapIsOnProperty
        };

        public ToggleHandler()
        {
            this.ValueChanged += HandleValueChanged;
        }

        void HandleValueChanged(object sender, EventArgs e) => toggle?.IsOnChanged?.Invoke(On);

        public UIView View => this;

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

        public static bool MapIsOnProperty(UISwitch nativeView, Toggle virtualView)
        {
            nativeView.On = virtualView.IsOn;
            return true;
        }
    }
}