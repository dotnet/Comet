using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using UIKit;

namespace Comet.iOS.Handlers
{
	class RadioGroupHandler : AbstractLayoutHandler
	{
		private List<CUIRadioButton> RadioButtons = new List<CUIRadioButton>();

		public RadioGroupHandler(CGRect rect) : base(rect)
		{
		}

		public RadioGroupHandler() : base()
		{
		}

		public override void SubviewAdded(UIView uiview)
		{
			base.SubviewAdded(uiview);

			var radioButton = (CUIRadioButton)uiview;
			radioButton.IsCheckedChanged += HandleIsCheckedChanged;
			RadioButtons.Add(radioButton);
		}
		
		public override void WillRemoveSubview(UIView uiview)
		{
			base.WillRemoveSubview(uiview);

			var radioButton = (CUIRadioButton)uiview;
			radioButton.IsCheckedChanged -= HandleIsCheckedChanged;
			RadioButtons.Remove(radioButton);
		}

		protected override void Dispose(bool disposing)
		{
			foreach(var button in RadioButtons)
			{
				button.IsCheckedChanged -= HandleIsCheckedChanged;
			}

			RadioButtons.Clear();

			base.Dispose(disposing);
		}

		private void HandleIsCheckedChanged(object sender, System.EventArgs e)
		{
			var changedButton = (CUIRadioButton)sender;

			if (changedButton.IsChecked)
			{
				foreach(var button in RadioButtons.Where(_ => _ != changedButton))
				{
					button.IsChecked = false;
				}
			}
		}
	}
}