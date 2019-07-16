using System;
using Android.Content;
using AButton = Android.Widget.Button;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class ButtonHandler : AbstractHandler<Button, AButton>
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

        private void HandleClick(object sender, EventArgs e) => VirtualView?.OnClick();
        
        public static void MapTextProperty(IViewHandler viewHandler, Button virtualView)
        {
            var nativeView = (AButton) viewHandler.NativeView;
            nativeView.Text = virtualView.Text;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            if (TypedNativeView != null)
                TypedNativeView.Click -= HandleClick;
            base.Dispose(disposing);
        }
    }
}