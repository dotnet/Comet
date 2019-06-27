using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using UWPButton = Windows.UI.Xaml.Controls.Button;

namespace HotUI.UWP
{
    public class ButtonHandler : UWPButton, IUIElement
    {
        private static readonly PropertyMapper<Button, ButtonHandler> Mapper = new PropertyMapper<Button, ButtonHandler>(new Dictionary<string, Func<ButtonHandler, Button, bool>>()
        {
            [nameof(Button.Text)] = MapTextProperty
        });
        
        private Button _button;

        public ButtonHandler()
        {
            Click += HandleClick;
        }
        

        public UIElement View => this;
        
        public void Remove(View view)
        {
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

        public static bool MapTextProperty(UWPButton nativeButton, Button virtualButton)
        {
            nativeButton.Content = virtualButton.Text;
            return true;
        }
    }
}