using System;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class ButtonHandler : NSButton, INSView
    {
        public ButtonHandler()
        {
            Activated += ButtonHandler_TouchUpInside;
        }

        private void ButtonHandler_TouchUpInside(object sender, EventArgs e) => button?.OnClick();

        public NSView View => this;

        Button button;

        public void Remove(View view)
        {
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
        public static void UpdateProperties(this NSButton view, Button hView)
        {
            view.Title = hView?.Text;
            view.UpdateBaseProperties(hView);
        }

        public static bool UpdateProperty(this NSButton view, string property, object value)
        {
            switch (property)
            {
                case nameof(Button.Text):
                    view.Title = (string) value;
                    return true;
            }

            return view.UpdateBaseProperty(property, value);
        }
    }
}