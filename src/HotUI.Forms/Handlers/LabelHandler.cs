using System;
using HotUI;
using Xamarin.Forms;
using FLabel = Xamarin.Forms.Label;
using HLabel = HotUI.Label;
using HView = HotUI.View;

namespace HotUI.Forms {
	public class LabelHandler : FLabel, IViewHandler, IFormsView {
		public Xamarin.Forms.View View => this;

		public void Remove (HView view)
		{
		}
		public void SetView (HView view)
		{
			var label = view as HLabel;
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
