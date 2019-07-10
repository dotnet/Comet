using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using UWPTextField = Windows.UI.Xaml.Controls.TextBox;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.UWP.Handlers
{
    public class TextFieldHandler : UWPTextField, IUIElement
    {
        public static readonly PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>()
        {
            [nameof(TextField.Text)] = MapTextProperty
        };
        
        private TextField _textField;
        
        public UIElement View => this;

        public object NativeView => View;

        public bool HasContainer
        {
            get => false;
            set { }
        }

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
        
        public static void MapTextProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var nativeView = (UWPTextField)viewHandler.NativeView;
            nativeView.Text = virtualView.Text;
        }
    }
}