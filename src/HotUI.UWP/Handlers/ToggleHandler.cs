using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HotUI.UWP.Handlers
{
    public class ToggleHandler :  AbstractControlHandler<Toggle, ToggleSwitch>
    {
		public static readonly PropertyMapper<Toggle> Mapper = new PropertyMapper<Toggle> ()
		{ 
            [nameof(Toggle.IsOn)] = MapIsOnProperty
        };

        public ToggleHandler() : base(Mapper)
        {

        }

        protected override ToggleSwitch CreateView()
        {
            var toggleSwitch = new ToggleSwitch();
            toggleSwitch.Toggled += HandleToggled;
            return toggleSwitch;
        }

        protected override void DisposeView(ToggleSwitch toggleSwitch)
        {
            toggleSwitch.Toggled += HandleToggled;
        }

        private void HandleToggled(object sender, RoutedEventArgs e) => VirtualView?.IsOnChanged?.Invoke(TypedNativeView.IsOn);

        public static void MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
        {
            var nativeView = (ToggleSwitch)viewHandler.NativeView;
            nativeView.IsOn = virtualView.IsOn;
        }
    }
}