using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace HotUI.UWP
{
    public class ToggleHandler :  IUIElement
    {
		public static readonly PropertyMapper<Toggle> Mapper = new PropertyMapper<Toggle> ()
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

        public object NativeView => View;

        public bool HasContainer
        {
            get => false;
            set { }
        }
        public void Remove(View view)
        {
            _virtualToggle = null;
        }

        public void SetView(View view)
        {
            _virtualToggle = view as Toggle;
            Mapper.UpdateProperties(this, _virtualToggle);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _virtualToggle, property);
        }

        public static bool MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
        {
            var nativeView = (ToggleSwitch)viewHandler.NativeView;
            nativeView.IsOn = virtualView.IsOn;
            return true;
        }
    }
}