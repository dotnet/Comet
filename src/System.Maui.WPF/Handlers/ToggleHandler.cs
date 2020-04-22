using System.Windows;
using System.Windows.Controls;

namespace System.Maui.WPF.Handlers
{
	public class ToggleHandler : AbstractControlHandler<Switch, CheckBox>
	{
		public static readonly PropertyMapper<Switch> Mapper = new PropertyMapper<Switch>()
		{
			[nameof(Switch.IsOn)] = MapIsOnProperty
		};

		public ToggleHandler() : base(Mapper)
		{

		}

		protected override CheckBox CreateView()
		{
			var checkbox = new CheckBox();
			checkbox.Checked += HandleCheckedChanged;
			checkbox.Unchecked += HandleCheckedChanged;
			return checkbox;
		}

		protected override void DisposeView(CheckBox checkbox)
		{
			checkbox.Checked -= HandleCheckedChanged;
			checkbox.Unchecked -= HandleCheckedChanged;
		}

		private void HandleCheckedChanged(object sender, RoutedEventArgs e) => VirtualView?.IsOnChanged?.Invoke(TypedNativeView.IsChecked ?? false);

		public static void MapIsOnProperty(IViewHandler viewHandler, Switch virtualView)
		{
			var nativeView = (CheckBox)viewHandler.NativeView;
			nativeView.IsChecked = virtualView.IsOn;
		}
	}
}
