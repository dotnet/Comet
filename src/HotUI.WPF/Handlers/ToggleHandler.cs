using System;
using System.Windows;
using System.Windows.Controls;


namespace HotUI.WPF
{
    public class ToggleHandler :  IUIElement
    {
		private static readonly PropertyMapper<Toggle, CheckBox> Mapper = new PropertyMapper<Toggle, CheckBox> ()
		{ 
            [nameof(Toggle.IsOn)] = MapIsOnProperty
        };

        private CheckBox _nativeCheckbox;
        private Toggle _virtualToggle;

        public ToggleHandler()
        {
            _nativeCheckbox = new CheckBox();
            _nativeCheckbox.Checked += HandleChecked;
        }

        private void HandleChecked(object sender, RoutedEventArgs e) => _virtualToggle?.IsOnChanged?.Invoke(_nativeCheckbox.IsChecked ?? false);

        public UIElement View => _nativeCheckbox;

        public void Remove(View view)
        {
            _virtualToggle = null;
        }

        public void SetView(View view)
        {
            _virtualToggle = view as Toggle;
            Mapper.UpdateProperties(_nativeCheckbox, _virtualToggle);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(_nativeCheckbox, _virtualToggle, property);
        }

        public static bool MapIsOnProperty(CheckBox nativeView, Toggle virtualView)
        {
            nativeView.IsChecked = virtualView.IsOn;
            return true;
        }
    }
}