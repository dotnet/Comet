using WPFRadioButton = System.Windows.Controls.RadioButton;

namespace Comet.WPF.Handlers
{
	public class RadioButtonHandler : AbstractControlHandler<RadioButton, WPFRadioButton>
	{
		public static readonly PropertyMapper<RadioButton> Mapper = new PropertyMapper<RadioButton>()
		{
			[nameof(RadioButton.Label)] = MapLabelProperty,
			[nameof(RadioButton.Selected)] = MapSelectedProperty
		};

		public RadioButtonHandler() : base(Mapper)
		{
		}

		protected override WPFRadioButton CreateView()
		{
			var radioButton = new WPFRadioButton();
			radioButton.Click += HandleClick;
			return radioButton;
		}

		protected override void DisposeView(WPFRadioButton nativeView)
		{
			nativeView.Click -= HandleClick;
		}

		private void HandleClick(object sender, System.Windows.RoutedEventArgs e)
		{
			VirtualView?.OnClick?.Invoke();
		}

		public static void MapLabelProperty(IViewHandler viewHandler, RadioButton virtualRadioButton)
		{
			var nativeRadioButton = (WPFRadioButton)viewHandler.NativeView;
			nativeRadioButton.Content = virtualRadioButton.Label.CurrentValue;
		}

		public static void MapSelectedProperty(IViewHandler viewHandler, RadioButton virtualRadioButton)
		{
			var nativeRadioButton = (WPFRadioButton)viewHandler.NativeView;
			nativeRadioButton.IsChecked = virtualRadioButton.Selected;
		}
	}
}
