using Xamarin.Forms;

namespace HotUI.Forms
{
	public class VStackHandler : AbstractFormsLayoutHandler, IFormsView
    {
        public VStackHandler() : base(new Xamarin.Forms.StackLayout() { Orientation = StackOrientation.Vertical})
        {

        }
	}
}
