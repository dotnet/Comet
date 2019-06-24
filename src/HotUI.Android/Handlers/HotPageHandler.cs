using Android.App;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class HotPageHandler : IViewContainer
    {
        HotPage hotpage;
        AView currentView;
        
        public AView View => currentView;

        public void Remove(View view)
        {
            // todo: implement this
        }

        public void SetView(View view)
        {
            currentView = view.ToView();
        }


        public void SetViewBuilder(ViewBuilder builder)
        {
            hotpage = builder as HotPage;
            if (hotpage.View == null)
                hotpage.ReBuildView();
            
            // todo: needs to be implemented
        }

        public void UpdateValue(string property, object value)
        {
            // todo: needs to be implemented
        }
    }
}