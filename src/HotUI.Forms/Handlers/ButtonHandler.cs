using System;
using HotUI;
using Xamarin.Forms;
using FButton = Xamarin.Forms.Button;
using HButton = HotUI.Button;
using HView = HotUI.View;
namespace HotUI.Forms {
	public class ButtonHandler : FButton, IViewHandler, IFormsView {

		HButton button;
		public Xamarin.Forms.View View => this;

		public void Remove (HView view)
		{
			button = null;
			Command = null;
		}

		public void SetView (HView view)
		{
			button = view as HButton;
			//Maybe we should throw an exception?
			if (button == null)
				return;
			Command = new Command ((s) => button.OnClick?.Invoke ());
			this.UpdateProperties (button);
		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property, value);
		}
	}
}
