using System;
using Xamarin.Forms;
using FToggle = Xamarin.Forms.Switch;

namespace HotUI.Forms.Handlers
{
    public class ToggleHandler : AbstractHandler<Toggle, FToggle>
    {
        public static readonly PropertyMapper<Toggle> Mapper = new PropertyMapper<Toggle>()
        {
            [nameof(Toggle.IsOn)] = MapIsOnProperty
        };

        public ToggleHandler() : base(Mapper)
        {
        }

        protected override FToggle CreateView()
        {
            var toggle = new FToggle();
            toggle.Toggled += HandleToggled;
            return toggle;
        }

        protected override void DisposeView(Switch toggle)
        {
            toggle.Toggled -= HandleToggled;
        }

        void HandleToggled(object sender, EventArgs e) => VirtualView?.IsOnChanged?.Invoke(TypedNativeView.IsToggled);


        public static void MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
        {
            var nativeView = (FToggle)viewHandler.NativeView;
            nativeView.IsToggled = virtualView.IsOn;
        }
    }
}