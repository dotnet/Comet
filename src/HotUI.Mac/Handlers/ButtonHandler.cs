using System;
using System.Collections.Generic;
using AppKit;

namespace HotUI.Mac.Handlers
{
    public class ButtonHandler : NSButton, INSView
    {
        public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>(ViewHandler.Mapper)
        {
            [nameof(Button.Text)] = MapTextProperty
        };
        
        public ButtonHandler()
        {
            Cell.ControlSize = NSControlSize.Regular;
            BezelStyle = NSBezelStyle.Rounded;
            SetButtonType(NSButtonType.MomentaryPushIn);
            Font = NSFont.SystemFontOfSize(NSFont.SystemFontSizeForControlSize(NSControlSize.Regular));

            Activated += HandleTouchUpInside;
        }

        private void HandleTouchUpInside(object sender, EventArgs e) => _button?.OnClick();

        public NSView View => this;

        public object NativeView => View;
        public bool HasContainer { get; set; } = false;

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

        public static bool MapTextProperty(IViewHandler viewHandler, Button virtualButton)
        {
            var nativeButton = (NSButton) viewHandler.NativeView;
            nativeButton.Title = virtualButton.Text;
            nativeButton.SizeToFit();
            return true;
        }
    }
}