﻿//using System;
//using Microsoft.Maui;
//using Microsoft.Maui.Handlers;
//using UIKit;

//namespace Comet.iOS.Handlers
//{
//	class RadioButtonHandler : ViewHandler<RadioButton, CUIRadioButton>
//	{
//		public static readonly PropertyMapper<RadioButton> Mapper = new PropertyMapper<RadioButton>(ViewHandler.ViewMapper)
//		{
//			[nameof(RadioButton.Label)] = MapLabelProperty,
//			[nameof(RadioButton.Selected)] = MapSelectedProperty
//		};

//		public RadioButtonHandler() : base(Mapper)
//		{
//		}

//		protected override CUIRadioButton CreateView()
//		{
//			var button = new CUIRadioButton(VirtualView.Selected);

//			button.TouchUpInside += HandleTouchUpInside;

//			return button;
//		}
		
//		protected override void DisposeView(CUIRadioButton nativeView)
//		{
//			nativeView.TouchUpInside -= HandleTouchUpInside;
//		}

//		private void HandleTouchUpInside(object sender, EventArgs e) => VirtualView?.OnClick?.Invoke();

//		public static void MapLabelProperty(IViewHandler viewHandler, RadioButton virtualRadioButton)
//		{
//			var nativeRadioButton = (CUIRadioButton)viewHandler.PlatformView;
//			nativeRadioButton.SetTitle(virtualRadioButton.Label?.CurrentValue, UIControlState.Normal);
//			virtualRadioButton.InvalidateMeasurement();
//		}

//		public static void MapSelectedProperty(IViewHandler viewHandler, RadioButton virtualRadioButton)
//		{
//			var nativeRadioButton = (CUIRadioButton)viewHandler.PlatformView;
//			nativeRadioButton.IsChecked = virtualRadioButton.Selected;
//		}
//	}
//}