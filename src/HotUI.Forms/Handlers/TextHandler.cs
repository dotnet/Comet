using System;
using HotUI;
using Xamarin.Forms;
using FLabel = Xamarin.Forms.Label;
using HView = HotUI.View;

namespace HotUI.Forms {
	public class TextHandler : FLabel, FormsViewHandler {
		public Xamarin.Forms.View View => this;
		public object NativeView => View;
		public bool HasContainer { get; set; } = false;

		public void Remove (HView view)
		{
		}
		public void SetView (HView view)
		{
			var label = view as Text;
			if (label == null) {
				return;
			}
			this.UpdateProperties (label);
		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property, value);
		}
	}
}
