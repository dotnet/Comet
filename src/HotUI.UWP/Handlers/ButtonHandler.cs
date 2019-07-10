using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using UWPButton = Windows.UI.Xaml.Controls.Button;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.UWP.Handlers
{
    public class ButtonHandler : UWPButton, IUIElement
    {
        public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>()
        {
            [nameof(Button.Text)] = MapTextProperty
        };
        
        private Button _button;

        public ButtonHandler()
        {
            Click += HandleClick;
        }
        

        public UIElement View => this;

        public object NativeView => View;

        public bool HasContainer
        {
            get => false;
            set { }
        }

        public void Remove(View view)
        {
            _button = null;
        }

        public void SetView(View view)
        {
            _button = view as Button;
            Mapper.UpdateProperties(this, _button);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _button, property);
        }

        private void HandleClick(object sender, RoutedEventArgs e) => _button?.OnClick();

        public static void MapTextProperty(IViewHandler viewHandler, Button virtualButton)
        {
            var nativeButton = (UWPButton)viewHandler.NativeView;
            nativeButton.Content = virtualButton.Text;
        }
    }
}