using System;
using System.Collections.Generic;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class TextFieldHandler : NSTextField, INSView
    {
        public static readonly PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>(ViewHandler.Mapper)
        {
            [nameof(TextField.Text)] = MapTextProperty
        };
        
        public TextFieldHandler()
        {
            EditingEnded += EntryHandler_Ended;
        }

        void EntryHandler_Ended(object sender, EventArgs e) => _textField?.Completed(StringValue);

        public NSView View => this;

        public object NativeView => View;
        public bool HasContainer { get; set; } = false;

        public void Remove(View view)
        {
            _textField = null;
        }

        TextField _textField;

        public void SetView(View view)
        {
            _textField = view as TextField;
            Mapper.UpdateProperties(this, _textField);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _textField, property);
        }
        
        public static bool MapTextProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var nativeView = (NSTextField) viewHandler.NativeView;
            nativeView.StringValue = virtualView.Text;
            nativeView.SizeToFit();
            return true;
        }
    }
}