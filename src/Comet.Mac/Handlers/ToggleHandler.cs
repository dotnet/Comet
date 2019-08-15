using System;
using AppKit;

// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Mac.Handlers
{
    public class ToggleHandler : AbstractControlHandler<Toggle, NSButton>
    {
		public static readonly PropertyMapper<Toggle> Mapper = new PropertyMapper<Toggle> (ViewHandler.Mapper)
		{ 
            [nameof(Toggle.IsOn)] = MapIsOnProperty
        };

        public ToggleHandler() : base(Mapper)
        {
        }

        protected override NSButton CreateView()
        {
            var toggle = new NSButton();
            toggle.SetButtonType(NSButtonType.Switch);
            toggle.Activated += HandleValueChanged;
            return toggle;
        }

        protected override void DisposeView(NSButton toggle)
        {
            toggle.Activated -= HandleValueChanged;
        }

        void HandleValueChanged(object sender, EventArgs e) => VirtualView?.IsOnChanged?.Invoke(TypedNativeView.State == NSCellStateValue.On);
        
        public static void MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
        {
            var nativeView = (NSButton) viewHandler.NativeView;
            nativeView.State = virtualView.IsOn ? NSCellStateValue.On : NSCellStateValue.Off;
        }
    }
}
