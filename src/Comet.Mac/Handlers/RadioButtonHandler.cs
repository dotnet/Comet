using AppKit;

namespace Comet.Mac.Handlers
{
	public class RadioButtonHandler : AbstractControlHandler<RadioButton, NSButton>
	{
		public static readonly PropertyMapper<RadioButton> Mapper = new PropertyMapper<RadioButton>()
		{
			[nameof(RadioButton.Label)] = MapLabelProperty,
			[nameof(RadioButton.Selected)] = MapSelectedProperty
		};

		public RadioButtonHandler() : base(Mapper)
		{
		}

		protected override NSButton CreateView()
		{
			var button = NSButton.CreateRadioButton(VirtualView.Label?.CurrentValue, RadioAction);

			button.State = VirtualView.Selected
				? NSCellStateValue.On
				: NSCellStateValue.Off;

			button.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSizeForControlSize(NSControlSize.Regular));
			button.Activated += HandleTouchUpInside;

			return button;
		}

		private void RadioAction()
		{

		}

		protected override void DisposeView(NSButton nativeView)
		{
			nativeView.Activated -= HandleTouchUpInside;
		}

		private void HandleTouchUpInside(object sender, System.EventArgs e)
		{
			VirtualView?.OnClick?.Invoke();
		}

		public static void MapLabelProperty(IViewHandler viewHandler, RadioButton virtualRadioButton)
		{
			var nativeRadioButton = (NSButton)viewHandler.NativeView;
			nativeRadioButton.Title = virtualRadioButton.Label?.CurrentValue;
		}

		public static void MapSelectedProperty(IViewHandler viewHandler, RadioButton virtualRadioButton)
		{
			var nativeRadioButton = (NSButton)viewHandler.NativeView;
			nativeRadioButton.State = virtualRadioButton.Selected
				? NSCellStateValue.On
				: NSCellStateValue.Off;
		}
	}
}
