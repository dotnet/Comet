using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers {
	public class LabelHandler : NSTextField, IViewHandler, INSView {

		public NSView View => this;

		public LabelHandler()
		{
			Editable = false;
		}
		
		public void Remove (View view)
		{

		}

		public void SetView (View view)
		{
			var label = view as Label;
			this.UpdateLabelProperties (label);

		}

		public void UpdateValue (string property, object value)
		{
			this.UpdateLabelProperty (property, value);
		}
	}

	public static partial class ControlExtensions {

		public static void UpdateLabelProperties (this NSTextField view, Label hView)
		{
			view.StringValue = hView?.Text;
			view.UpdateBaseProperties (hView);
		}

		public static bool UpdateLabelProperty (this NSTextField view, string property, object value)
		{
			switch (property) {
			case nameof (Label.Text):
				view.StringValue = (string)value;
				return true;
			}
			return view.UpdateBaseProperty (property, value);
		}
	}
}
