using System;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class TextFieldHandler : NSTextField, INSView
    {
        public TextFieldHandler()
        {
            EditingEnded += EntryHandler_Ended;
        }

        void EntryHandler_Ended(object sender, EventArgs e) => _textField?.Completed(StringValue);

        public NSView View => this;

        public void Remove(View view)
        {
            _textField = null;
        }

        TextField _textField;

        public void SetView(View view)
        {
            _textField = view as TextField;
            this.UpdateTextFieldProperties(_textField);
        }

        public void UpdateValue(string property, object value)
        {
            this.UpdateTextFieldProperty(property, value);
        }
    }

    public static partial class ControlExtensions
    {
        public static void UpdateTextFieldProperties(this NSTextField view, TextField hView)
        {
            view.UpdateLabelProperty(nameof(Text.Value), hView?.Text);
            view.UpdateBaseProperties(hView);
        }

        public static bool UpdateTextFieldProperty(this NSTextField view, string property, object value)
        {
            switch (property)
            {
                case nameof(TextField.Text):
                    view.StringValue = (string) value;
                    view.SizeToFit();
                    return true;
            }

            return view.UpdateBaseProperty(property, value);
        }
    }
}