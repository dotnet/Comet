using System;
using UIKit;

namespace Comet.iOS.Handlers
{
	class RadioButtonHandler : AbstractControlHandler<RadioButton, CUIRadioButton>
	{
		public static readonly PropertyMapper<RadioButton> Mapper = new PropertyMapper<RadioButton>(ViewHandler.Mapper)
		{
			[nameof(RadioButton.Label)] = MapLabelProperty,
			[nameof(RadioButton.Selected)] = MapSelectedProperty
		};

		public RadioButtonHandler() : base(Mapper)
		{
		}

		protected override CUIRadioButton CreateView()
		{
			var button = new CUIRadioButton(VirtualView.Selected);

			button.TouchUpInside += HandleTouchUpInside;

			return button;
		}
		
		protected override void DisposeView(CUIRadioButton nativeView)
		{
			nativeView.TouchUpInside -= HandleTouchUpInside;
		}

		private void HandleTouchUpInside(object sender, EventArgs e) => VirtualView?.OnClick?.Invoke();

		public static void MapLabelProperty(IViewHandler viewHandler, RadioButton virtualRadioButton)
		{
			var nativeRadioButton = (CUIRadioButton)viewHandler.NativeView;
			nativeRadioButton.SetTitle(virtualRadioButton.Label?.CurrentValue, UIControlState.Normal);
			virtualRadioButton.InvalidateMeasurement();
		}

		public static void MapSelectedProperty(IViewHandler viewHandler, RadioButton virtualRadioButton)
		{
			var nativeRadioButton = (CUIRadioButton)viewHandler.NativeView;
			nativeRadioButton.IsChecked = virtualRadioButton.Selected;
		}
	}
}