using Xamarin.Forms;

namespace Comet.Forms.Handlers
{
    public class HStackHandler : AbstractFormsLayoutHandler, FormsViewHandler
    {
        public HStackHandler() : base(new Xamarin.Forms.StackLayout() { Orientation = StackOrientation.Horizontal })
        {

        }
    }
}
