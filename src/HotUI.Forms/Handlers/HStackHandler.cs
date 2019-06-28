using System;
using Xamarin.Forms;

namespace HotUI.Forms
{
    public class HStackHandler : AbstractFormsLayoutHandler, IFormsView
    {
        public HStackHandler() : base(new Xamarin.Forms.StackLayout() { Orientation = StackOrientation.Horizontal })
        {

        }
    }
}
