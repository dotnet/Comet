using System;
using UIKit;

namespace HotUI.iOS
{
    public class ToggleHandler : AbstractHandler<Toggle, UISwitch>
    {
		public static readonly PropertyMapper<Toggle> Mapper = new PropertyMapper<Toggle> (ViewHandler.Mapper)
		{ 
            [nameof(Toggle.IsOn)] = MapIsOnProperty
        };

        public ToggleHandler() : base(Mapper)
        {
        }

        protected override UISwitch CreateView()
        {
            var toggle = new UISwitch();
            toggle.ValueChanged += HandleValueChanged;
            return toggle;
        }
        
        void HandleValueChanged(object sender, EventArgs e) => VirtualView?.IsOnChanged?.Invoke(TypedNativeView.On);
        
        public static void MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
        {
            var nativeView = (UISwitch) viewHandler.NativeView;
            nativeView.On = virtualView.IsOn;
        }
    }
}