using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers {
	public class ScrollViewHandler : NSScrollView, INSView {
		public NSView View => this;

		public void Remove (View view)
		{
			content?.RemoveFromSuperview ();
		}
		NSView content;
		public void SetView (View view)
		{
			var scroll = view as ScrollView;

			content = scroll?.View?.ToView ();
			if(content != null)
				AddSubview(content);
			this.UpdateProperties (view);
			NSLayoutConstraint.ActivateConstraints (new []{
				content.LeadingAnchor.ConstraintEqualToAnchor (this.LeadingAnchor),
				content.TrailingAnchor.ConstraintEqualToAnchor (this.TrailingAnchor),
				content.TopAnchor.ConstraintEqualToAnchor (this.TopAnchor),
				content.BottomAnchor.ConstraintEqualToAnchor (this.BottomAnchor),
			});

		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateProperty (property, value);
		}
	}
}
