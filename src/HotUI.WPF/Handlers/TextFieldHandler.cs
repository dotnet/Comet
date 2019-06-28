using System;
using System.Collections.Generic;
using System.Windows;
using WPFTextField = System.Windows.Controls.TextBox;

namespace HotUI.WPF.Handlers
{
    public class TextFieldHandler : WPFTextField, IUIElement
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
            /*RenderSize = new Size(100, 24);
            Width = RenderSize.Width;
            Height = RenderSize.Height;*/
            Mapper.UpdateProperties(this, _textField);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _textField, property);
        }
        
        public static bool MapTextProperty(WPFTextField nativeView, TextField virtualView)
        {
            nativeView.Text = virtualView.Text;
            //nativeView.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //var desiredSize = nativeView.DesiredSize;
            return true;
        }
    }
}