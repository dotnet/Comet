using Xamarin.Forms;

namespace Comet.Forms.Handlers
{
    public class VStackHandler : AbstractFormsLayoutHandler, FormsViewHandler
    {
        public VStackHandler() : base(new Xamarin.Forms.StackLayout() { Orientation = StackOrientation.Vertical })
        {

        }
    }
}
