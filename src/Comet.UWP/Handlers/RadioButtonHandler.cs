using UWPRadioButton = Windows.UI.Xaml.Controls.RadioButton;

namespace Comet.UWP.Handlers
{
	public class RadioButtonHandler : AbstractControlHandler<RadioButton, UWPRadioButton>
	{
		public static readonly PropertyMapper<RadioButton> Mapper = new PropertyMapper<RadioButton>()
		{
			[nameof(RadioButton.Label)] = MapLabelProperty,
			[nameof(RadioButton.Selected)] = MapSelectedProperty
		};

		public RadioButtonHandler() : base(Mapper)
		{
		}

		protected override UWPRadioButton CreateView()
		{
			var radioButton = new UWPRadioButton();
			radioButton.Click += HandleClick;
			return radioButton;
		}

		protected override void DisposeView(UWPRadioButton nativeView)
		{
			nativeView.Click -= HandleClick;
		}

		private void HandleClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			VirtualView?.OnClick?.Invoke();
		}

		public static void MapLabelProperty(IViewHandler viewHandler, RadioButton virtualRadioButton)
		{
			var nativeRadioButton = (UWPRadioButton)viewHandler.NativeView;
			nativeRadioButton.Content = virtualRadioButton.Label.CurrentValue;
		}

		public static void MapSelectedProperty(IViewHandler viewHandler, RadioButton virtualRadioButton)
		{
			var nativeRadioButton = (UWPRadioButton)viewHandler.NativeView;
			nativeRadioButton.IsChecked = virtualRadioButton.Selected;
		}
	}
}
