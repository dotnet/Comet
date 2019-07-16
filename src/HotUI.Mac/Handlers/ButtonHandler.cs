using System;
using AppKit;

namespace HotUI.Mac.Handlers
{
    public class ButtonHandler : AbstractHandler<Button,NSButton>
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

        private void HandleTouchUpInside(object sender, EventArgs e) => VirtualView?.OnClick();
        
        public static void MapTextProperty(IViewHandler viewHandler, Button virtualButton)
        {
            var nativeButton = (NSButton) viewHandler.NativeView;
            nativeButton.Title = virtualButton.Text;
            nativeButton.SizeToFit();
        }
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            if(TypedNativeView != null)
                TypedNativeView.Activated -= HandleTouchUpInside;
            base.Dispose(disposing);
        }
    }
}