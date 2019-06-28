using System;
using System.Collections.Generic;
using AppKit;

namespace HotUI.Mac.Handlers
{
    public class ButtonHandler : NSButton, INSView
    {
        private static readonly PropertyMapper<Button, ButtonHandler> Mapper = new PropertyMapper<Button, ButtonHandler>()
        {
            [nameof(Button.Text)] = MapTextProperty
        };
        
        public ButtonHandler()
        {
            Activated += HandleTouchUpInside;
        }

        private void HandleTouchUpInside(object sender, EventArgs e) => _button?.OnClick();

        public NSView View => this;

        Button _button;

        public void Remove(View view)
        {
        }

        public void SetView(View view)
        {
            _button = view as Button;
            Mapper.UpdateProperties(this, _button);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _button, property);
        }

        public static bool MapTextProperty(NSButton nativeButton, Button virtualButton)
        {
            nativeButton.Title = virtualButton.Text;
            nativeButton.SizeToFit();
            return true;
        }
    }
}