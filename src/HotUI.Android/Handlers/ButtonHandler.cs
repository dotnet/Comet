using System;
using Android.Content;
using AButton = Android.Widget.Button;

// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.Android.Handlers
{
    public class ButtonHandler : AbstractControlHandler<Button, AButton>
    {
        public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>(ViewHandler.Mapper)
        {
            [nameof(Button.Text)] = MapTextProperty
        };

        public ButtonHandler() : base(Mapper)
        {
        }

        protected override AButton CreateView(Context context)
        {
            var button = new AButton(context);
            button.Click += HandleClick;
            return button;
        }

        protected override void DisposeView(AButton nativeView)
        {
            nativeView.Click -= HandleClick;
        }

        private void HandleClick(object sender, EventArgs e) => VirtualView?.OnClick();

        public static void MapTextProperty(IViewHandler viewHandler, Button virtualView)
        {
            var nativeView = (AButton) viewHandler.NativeView;
            nativeView.Text = virtualView.Text;
        }
    }
}