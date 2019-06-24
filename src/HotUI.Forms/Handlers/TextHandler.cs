using System;
using HotUI;
using Xamarin.Forms;
using FLabel = Xamarin.Forms.Label;
using HView = HotUI.View;

namespace HotUI.Forms {
	public class TextHandler : FLabel, IFormsView {
		public Xamarin.Forms.View View => this;

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
