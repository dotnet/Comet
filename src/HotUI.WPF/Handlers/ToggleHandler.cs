using System.Windows;
using System.Windows.Controls;

namespace HotUI.WPF.Handlers
{
    public class ToggleHandler :  AbstractControlHandler<Toggle, CheckBox>
    {
		public static readonly PropertyMapper<Toggle> Mapper = new PropertyMapper<Toggle> ()
		{ 
            [nameof(Toggle.IsOn)] = MapIsOnProperty
        };

        public ToggleHandler() : base(Mapper)
        {

        }
        
        protected override CheckBox CreateView()
        {
            var checkbox = new CheckBox();
            checkbox.Checked += HandleChecked;
            return checkbox;
        }

        protected override void DisposeView(CheckBox checkbox)
        {
            checkbox.Checked -= HandleChecked;
        }

        private void HandleChecked(object sender, RoutedEventArgs e) => VirtualView?.IsOnChanged?.Invoke(TypedNativeView.IsChecked ?? false);

        public static void MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
        {
            var nativeView = (CheckBox)viewHandler.NativeView;
            nativeView.IsChecked = virtualView.IsOn;
        }
    }
}