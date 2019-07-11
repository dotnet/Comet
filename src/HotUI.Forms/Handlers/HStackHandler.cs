using System;
using Xamarin.Forms;

namespace HotUI.Forms
{
    public class HStackHandler : AbstractFormsLayoutHandler, FormsViewHandler
    {
        public HStackHandler() : base(new Xamarin.Forms.StackLayout() { Orientation = StackOrientation.Horizontal })
        {

        }
    }
}
