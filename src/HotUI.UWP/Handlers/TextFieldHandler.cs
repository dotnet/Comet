using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using UWPTextField = Windows.UI.Xaml.Controls.TextBox;

namespace HotUI.UWP
{
    public class TextFieldHandler : UWPTextField, IUIElement
    {
        private static readonly PropertyMapper<TextField, TextFieldHandler> Mapper = new PropertyMapper<TextField, TextFieldHandler>(new Dictionary<string, Func<TextFieldHandler, TextField, bool>>()
        {
            [nameof(TextField.Text)] = MapTextProperty
        });
        
        private TextField _textField;
        
        public UIElement View => this;
        
        public void Remove(View view)
        {
        }

        public void SetView(View view)
        {
            _textField = view as TextField;
            Mapper.UpdateProperties(this, _textField);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _textField, property);
        }
        
        public static bool MapTextProperty(UWPTextField nativeView, TextField virtualView)
        {
            nativeView.Text = virtualView.Text;
            return true;
        }
    }
}