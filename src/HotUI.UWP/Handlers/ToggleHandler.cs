using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace HotUI.UWP
{
    public class ToggleHandler :  IUIElement
    {
		public static readonly PropertyMapper<Toggle, UIElement, ToggleSwitch> Mapper = new PropertyMapper<Toggle, UIElement, ToggleSwitch> ()
		{ 
            [nameof(Toggle.IsOn)] = MapIsOnProperty
        };

        private ToggleSwitch _nativeToggle;
        private Toggle _virtualToggle;

        public ToggleHandler()
        {
            _nativeToggle = new ToggleSwitch();
            _nativeToggle.Toggled += HandleToggled;
        }

        private void HandleToggled(object sender, RoutedEventArgs e) => _virtualToggle?.IsOnChanged?.Invoke(_nativeToggle.IsOn);

        public UIElement View => _nativeToggle;

        public void Remove(View view)
        {
            _virtualToggle = null;
        }

        public void SetView(View view)
        {
            _virtualToggle = view as Toggle;
            Mapper.UpdateProperties(_nativeToggle, _virtualToggle);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(_nativeToggle, _virtualToggle, property);
        }

        public static bool MapIsOnProperty(ToggleSwitch nativeView, Toggle virtualView)
        {
            nativeView.IsOn = virtualView.IsOn;
            return true;
        }
    }
}