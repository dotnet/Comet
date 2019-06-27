using System;
using System.Collections.Generic;
using System.Windows;
using WPFButton = System.Windows.Controls.Button;

namespace HotUI.WPF
{
    public class ButtonHandler : WPFButton, IUIElement
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

        private void HandleClick(object sender, EventArgs e) => _button?.OnClick();

        public static bool MapTextProperty(WPFButton nativeButton, Button virtualButton)
        {
            nativeButton.Content = virtualButton.Text;
            return true;
        }
    }
}