using HotUI.Blazor.Components;

namespace HotUI.Blazor.Handlers
{
    internal class ButtonHandler : BlazorHandler<Button, BButton>
    {
        public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>
        {
            { nameof(Button.Text), MapTextProperty },
            { nameof(Button.OnClick), MapOnClickProperty }
        };

        public ButtonHandler()
            : base(Mapper)
        {
        }

        public static void MapOnClickProperty(IViewHandler viewHandler, Button virtualView)
        {
            var nativeView = (BButton)viewHandler.NativeView;

            if (nativeView is null)
            {
                return;
            }

            nativeView.OnClick = virtualView.OnClick;
        }

        public static void MapTextProperty(IViewHandler viewHandler, Button virtualView)
        {
            var nativeView = (BButton)viewHandler.NativeView;

            if (nativeView is null)
            {
                return;
            }

            nativeView.Text = virtualView.Text;
        }
    }
}
