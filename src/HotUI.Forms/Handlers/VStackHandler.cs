using Xamarin.Forms;

namespace HotUI.Forms.Handlers
{
    public class VStackHandler : AbstractFormsLayoutHandler, FormsViewHandler
    {
        public VStackHandler() : base(new Xamarin.Forms.StackLayout() { Orientation = StackOrientation.Vertical })
        {

        }
    }
}
