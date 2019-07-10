using System;
using System.Collections.Generic;
using System.Windows;
using WPFTextField = System.Windows.Controls.TextBox;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.WPF.Handlers
{
    public class TextFieldHandler : WPFTextField, WPFViewHandler
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
            /*RenderSize = new Size(100, 24);
            Width = RenderSize.Width;
            Height = RenderSize.Height;*/
            Mapper.UpdateProperties(this, _textField);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _textField, property);
        }
        
        public static void MapTextProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var nativeView = (WPFTextField)viewHandler.NativeView;
            nativeView.Text = virtualView.Text;
            //nativeView.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //var desiredSize = nativeView.DesiredSize;
        }
    }
}