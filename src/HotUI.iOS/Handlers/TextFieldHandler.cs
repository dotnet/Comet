using System;
using UIKit;

namespace HotUI.iOS
{
    public class TextFieldHandler : UITextField, IUIView
    {
        public TextFieldHandler()
        {
            EditingDidEnd += EntryHandler_EditingDidEnd;

            ShouldReturn = (s) =>
            {
                ResignFirstResponder();
                return true;
            };
        }

        private void EntryHandler_EditingDidEnd(object sender, EventArgs e) => _textField?.Completed(Text);

        public UIView View => this;

        public void Remove(View view)
        {
            _textField = null;
        }

        TextField _textField;

        public void SetView(View view)
        {
            _textField = view as TextField;
            this.UpdateProperties(_textField);
        }

        public void UpdateValue(string property, object value)
        {
            this.UpdateProperty(property, value);
        }
    }

    public static partial class ControlExtensions
    {
        public static void UpdateProperties(this UITextField view, TextField hView)
        {
            view.UpdateProperty(nameof(TextField.Text), hView?.Text);
            view.UpdateBaseProperties(hView);
        }

        public static bool UpdateProperty(this UITextField view, string property, object value)
        {
            switch (property)
            {
                case nameof(TextField.Text):
                    view.Text = (string) value;
                    view.SizeToFit();
                    return true;
            }

            return view.UpdateBaseProperty(property, value);
        }
    }
}