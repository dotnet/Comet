using System;
using HotUI;
using Xamarin.Forms;
using FButton = Xamarin.Forms.Button;
using HButton = HotUI.Button;
using HView = HotUI.View;
namespace HotUI.Forms
{
    public class ButtonHandler : AbstractHandler<HButton, FButton>
    {
        public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>(ViewHandler.Mapper)
        {
            [nameof(Button.Text)] = MapTextProperty
        };

        public ButtonHandler() : base(Mapper)
        {
        }
        public static void MapTextProperty(IViewHandler viewHandler, Button virtualView)
        {
            var nativeView = (FButton)viewHandler.NativeView;
            nativeView.Text = virtualView.Text;
        }

        protected override FButton CreateView()
        {
            var button = new FButton();
            button.Command = new Command((s) => VirtualView?.OnClick?.Invoke());
            return button;
        }
    }
}
