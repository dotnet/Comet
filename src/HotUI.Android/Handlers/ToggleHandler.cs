using System;
using Android.Content;
using AToggle = Android.Widget.ToggleButton;

// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.Android.Handlers
{
    public class ToggleHandler : AbstractControlHandler<Toggle, AToggle>
    {
        public static readonly PropertyMapper<Toggle> Mapper = new PropertyMapper<Toggle>(ViewHandler.Mapper)
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

        protected override void DisposeView(AToggle nativeView)
        {
            nativeView.Click -= HandleClick;
        }

        private void HandleClick(object sender, EventArgs e) => VirtualView?.IsOnChanged?.Invoke(TypedNativeView.Checked);

        public static void MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
        {
            var nativeView = (AToggle) viewHandler.NativeView;
            nativeView.Checked = virtualView.IsOn;
        }
    }
}