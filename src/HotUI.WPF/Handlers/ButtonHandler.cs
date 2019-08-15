using System;
using WPFButton = System.Windows.Controls.Button;
// ReSharper disable ClassNeverInstantiated.Global

namespace Comet.WPF.Handlers
{
    public class ButtonHandler : AbstractControlHandler<Button, WPFButton>
    {
        public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>()
        {
            [nameof(Button.Text)] = MapTextProperty
        };
        

        public ButtonHandler() : base(Mapper)
        {
        }

        protected override WPFButton CreateView()
        {
            var button = new WPFButton();
            button.Click += HandleClick;
            return button;
        }

        protected override void DisposeView(WPFButton button)
        {
            button.Click -= HandleClick;
        }

        private void HandleClick(object sender, EventArgs e) => VirtualView?.OnClick();

        public static void MapTextProperty(IViewHandler viewHandler, Button virtualButton)
        {
            var nativeButton = (WPFButton)viewHandler.NativeView;
            nativeButton.Content = virtualButton.Text;
        }
    }
}