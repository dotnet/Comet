using System;
using System.Windows;
using System.Windows.Controls;


namespace HotUI.WPF
{
    public class ToggleHandler :  WPFViewHandler
    {
		public static readonly PropertyMapper<Toggle> Mapper = new PropertyMapper<Toggle> ()
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

        public static void MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
        {
            var nativeView = (CheckBox)viewHandler.NativeView;
            nativeView.IsChecked = virtualView.IsOn;
        }
    }
}