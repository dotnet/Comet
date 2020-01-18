using Android.Content;
using ARadioButton = Android.Widget.RadioButton;

namespace Comet.Android.Handlers
{
	public class RadioButtonHandler : AbstractControlHandler<RadioButton, ARadioButton>
	{
		public static readonly PropertyMapper<RadioButton> Mapper = new PropertyMapper<RadioButton>()
		{
			[nameof(RadioButton.Label)] = MapLabelProperty,
			[nameof(RadioButton.Selected)] = MapSelectedProperty
		};

		public RadioButtonHandler() : base(Mapper)
		{
		}

		protected override ARadioButton CreateView(Context context)
		{
			var radioButton = new ARadioButton(context);
			radioButton.Click += HandleClick;
			return radioButton;
		}

		protected override void DisposeView(ARadioButton nativeView)
		{
			nativeView.Click -= HandleClick;
		}

		private void HandleClick(object sender, System.EventArgs e)
		{
			VirtualView?.OnClick?.Invoke();
		}

		public static void MapLabelProperty(IViewHandler viewHandler, RadioButton virtualRadioButton)
		{
			var nativeRadioButton = (ARadioButton)viewHandler.NativeView;
			nativeRadioButton.Text = virtualRadioButton.Label?.CurrentValue;
		}

		public static void MapSelectedProperty(IViewHandler viewHandler, RadioButton virtualRadioButton)
		{
			var nativeRadioButton = (ARadioButton)viewHandler.NativeView;
			nativeRadioButton.Checked = virtualRadioButton.Selected;
		}
	}
}