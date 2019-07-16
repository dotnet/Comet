using System;
using System.Collections.Generic;
using Xamarin.Forms;
using FToggle = Xamarin.Forms.Switch;
using FView = Xamarin.Forms.View;

namespace HotUI.Forms
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

        void HandleToggled(object sender, EventArgs e) => VirtualView?.IsOnChanged?.Invoke(TypedNativeView.IsToggled);


        public static void MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
        {
            var nativeView = (FToggle)viewHandler.NativeView;
            nativeView.IsToggled = virtualView.IsOn;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                return;
            if(TypedNativeView != null)
                TypedNativeView.Toggled += HandleToggled;
            base.Dispose(disposing);
        }
    }
}