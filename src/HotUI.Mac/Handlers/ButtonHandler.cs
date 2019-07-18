using System;
using AppKit;
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.Mac.Handlers
{
    public class ButtonHandler : AbstractControlHandler<Button,NSButton>
    {
        public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>(ViewHandler.Mapper)
        {
            [nameof(Button.Text)] = MapTextProperty
        };
        
        public ButtonHandler() : base(Mapper)
        {

        }
        
        protected override NSButton CreateView()
        {
            var button = new NSButton();

            button.Cell.ControlSize = NSControlSize.Regular;
            button.BezelStyle = NSBezelStyle.Rounded;
            button.SetButtonType(NSButtonType.MomentaryPushIn);
            button.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSizeForControlSize(NSControlSize.Regular));
            button.Activated += HandleTouchUpInside;
            
            return button;
        }

        protected override void DisposeView(NSButton nativeView)
        {
            nativeView.Activated -= HandleTouchUpInside;
        }

        private void HandleTouchUpInside(object sender, EventArgs e) => VirtualView?.OnClick();
        
        public static void MapTextProperty(IViewHandler viewHandler, Button virtualButton)
        {
            var nativeButton = (NSButton) viewHandler.NativeView;
            nativeButton.Title = virtualButton.Text;
            nativeButton.SizeToFit();
        }
    }
}