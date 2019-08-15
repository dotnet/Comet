using Xamarin.Forms;
using FButton = Xamarin.Forms.Button;
using HButton = Comet.Button;

// ReSharper disable MemberCanBePrivate.Global
namespace Comet.Forms.Handlers
{
    public class ButtonHandler : AbstractControlHandler<HButton, FButton>
    {
        public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>(ViewHandler.Mapper)
        {
            [nameof(Button.Text)] = MapTextProperty
        };

        public ButtonHandler() : base(Mapper)
        {
        }
        
        protected override FButton CreateView()
        {
            var button = new FButton();
            button.Command = new Command((s) => VirtualView?.OnClick?.Invoke());
            return button;
        }

        protected override void DisposeView(FButton nativeView)
        {
            nativeView.Command = null;
        }

        public static void MapTextProperty(IViewHandler viewHandler, Button virtualView)
        {
            var nativeView = (FButton)viewHandler.NativeView;
            nativeView.Text = virtualView.Text;
        }
    }
}
