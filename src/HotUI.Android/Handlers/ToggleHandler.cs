using System;
using Android.Content;
using AToggle = Android.Widget.ToggleButton;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class ToggleHandler : AbstractHandler<Toggle,AToggle>
    {
        public static readonly PropertyMapper<Toggle> Mapper = new PropertyMapper<Toggle> (ViewHandler.Mapper)
        { 
            [nameof(Toggle.IsOn)] = MapIsOnProperty
        };
        
        public ToggleHandler() : base(Mapper)
        {
        }
        
        protected override AToggle CreateView(Context context)
        {
            var toggle = new AToggle(context);
            toggle.Click += HandleClick;
            return toggle;
        }
        
        private void HandleClick(object sender, EventArgs e) => VirtualView?.IsOnChanged?.Invoke(TypedNativeView.Checked);
        
        public static bool MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
        {
            var nativeView = (AToggle) viewHandler.NativeView;
            nativeView.Checked = virtualView.IsOn;
            return true;
        }
    }
}