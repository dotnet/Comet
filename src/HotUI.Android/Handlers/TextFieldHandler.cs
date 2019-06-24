using System;
using Android.Widget;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class TextFieldHandler : EditText, IView
    {
        public TextFieldHandler() : base(AndroidContext.CurrentContext)
        {
            TextChanged += HandleTextChanged;
        }
        
        private void HandleTextChanged(object sender, EventArgs e) => _textField?.Completed(Text);

        public AView View => this;

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
        public static void UpdateProperties(this EditText view, TextField hView)
        {
            view.Text = hView?.Text;
            view.UpdateBaseProperties(hView);
        }

        public static bool UpdateProperty(this EditText view, string property, object value)
        {
            switch (property)
            {
                case nameof(TextField.Text):
                    view.Text = (string) value;
                    return true;
            }

            return view.UpdateBaseProperty(property, value);
        }
    }
}