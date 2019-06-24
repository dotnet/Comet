using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class HotPageHandler : NSViewController, INSViewController
    {
        public NSViewController ViewController => this;

        NSView currentView;

        public void Remove(View view)
        {
            currentView?.RemoveFromSuperview();
        }

        public void SetView(View view)
        {
            var newView = view.ToView();
            if (newView != currentView)
            {
                currentView?.RemoveFromSuperview();
                currentView = view.ToView();
                View = currentView;
            }
        }

        HotPage hotpage;

        public void SetViewBuilder(ViewBuilder builder)
        {
            hotpage = builder as HotPage;
            if (hotpage.View == null)
                hotpage.ReBuildView();
            this.UpdateProperties(hotpage);
        }

        public void UpdateValue(string property, object value)
        {
            this.UpdateProperty(property, value);
        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();
            hotpage?.OnAppearing();
        }

        public override void ViewWillDisappear()
        {
            base.ViewWillDisappear();
            hotpage?.OnDisppearing();
        }
    }

    public static partial class ControlExtensions
    {
        public static void UpdateProperties(this NSViewController view, HotPage hView)
        {
            view.Title = hView.Title;
        }

        public static bool UpdateProperty(this NSViewController view, string property, object value)
        {
            switch (property)
            {
                case nameof(HotPage.Title):
                    view.Title = (string) value;
                    return true;
            }

            return false;
        }
    }
}