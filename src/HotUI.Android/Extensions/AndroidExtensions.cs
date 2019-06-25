using Android.App;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public static partial class AndroidExtensions
    {
        static AndroidExtensions()
        {
            HotUI.Android.UI.Init();
        }

        /*public static Activity ToActivity(this HotPage hotPage)
        {
            if (hotPage == null)
                return null;
            var handler = hotPage.ViewHandler;
            if (handler == null)
            {
                handler = Registrar.Pages.GetRenderer(hotPage.GetType()) as IViewContainer;
                hotPage.ViewHandler = handler;
                hotPage.ReBuildView();
            }

            var page = handler as IViewContainer;
            return page.Activity;
        }*/

        public static AView ToView(this View view)
        {
            if (view == null)
                return null;
            var handler = view.ViewHandler;
            if (handler == null)
            {
                handler = Registrar.Handlers.GetRenderer(view.GetType()) as IViewHandler;
                view.ViewHandler = handler;
            }

            var page = handler as IView;
            return page.View;
        }
    }
}