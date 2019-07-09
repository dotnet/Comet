using System;
using HotUI.iOS.Controls;
using UIKit;

namespace HotUI.iOS
{
    public class ToggleHandler : UISwitch, iOSViewHandler
    {
		public static readonly PropertyMapper<Toggle> Mapper = new PropertyMapper<Toggle> (ViewHandler.Mapper)
		{ 
            [nameof(Toggle.IsOn)] = MapIsOnProperty
        };

        public ToggleHandler()
        {
            this.ValueChanged += HandleValueChanged;
        }

        void HandleValueChanged(object sender, EventArgs e) => toggle?.IsOnChanged?.Invoke(On);

        public UIView View => this;

        public HUIContainerView ContainerView => null;

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
            Mapper.UpdateProperties(this, toggle);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, toggle, property);
        }

        public static bool MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
        {
            var nativeView = (UISwitch) viewHandler.NativeView;
            nativeView.On = virtualView.IsOn;
            return true;
        }
    }
}