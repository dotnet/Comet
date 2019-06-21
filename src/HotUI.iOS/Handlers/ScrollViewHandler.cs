using System;
using UIKit;

namespace HotUI.iOS {
	public class ScrollViewHandler : UIScrollView, IUIView {
		public UIView View => this;

		public void Remove (View view)
		{
			content?.RemoveFromSuperview ();
		}
		UIView content;
		public void SetView (View view)
		{
			var scroll = view as ScrollView;

			content = scroll?.View?.ToView ();
			if(content != null)
				Add (content);
			this.UpdateProperties (view);
			NSLayoutConstraint.ActivateConstraints (new []{
				content.LeadingAnchor.ConstraintEqualTo (this.LeadingAnchor),
				content.TrailingAnchor.ConstraintEqualTo (this.TrailingAnchor),
				content.TopAnchor.ConstraintEqualTo (this.TopAnchor),
				content.BottomAnchor.ConstraintEqualTo (this.BottomAnchor),
			});

		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property, value);
		}
	}
}
