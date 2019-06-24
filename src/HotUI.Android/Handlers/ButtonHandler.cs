using System;
using AButton = Android.Widget.Button;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class ButtonHandler : AButton, IViewHandler, IView
    {
        public ButtonHandler() : base(AndroidContext.CurrentContext)
        {
            Click += HandleClick;
        }
        
        private void HandleClick(object sender, EventArgs e) => button?.OnClick();

        public AView View => this;

        Button button;

        public void Remove(View view)
        {
            button = null;
        }

        public void SetView(View view)
        {
            button = view as Button;
            this.UpdateProperties(button);
        }

        public void UpdateValue(string property, object value)
        {
            this.UpdateProperty(property, value);
        }
    }

    public static partial class ControlExtensions
    {
        public static void UpdateProperties(this AButton view, Button hView)
        {
            view.Text = hView?.Text;
            view.UpdateBaseProperties(hView);
        }

        public static bool UpdateProperty(this AButton view, string property, object value)
        {
            switch (property)
            {
                case nameof(Button.Text):
                    view.Text = (string) value;
                    return true;
            }

            return view.UpdateBaseProperty(property, value);
        }
    }
}