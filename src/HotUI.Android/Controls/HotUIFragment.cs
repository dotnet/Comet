using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using AView = Android.Views.View;

namespace HotUI.Android.Controls
{
    public class HotUIFragment : Fragment
    {
        private readonly View view;
        public HotUIFragment(View view)
        {
            this.view = view;
            
        }
        AView currentBuiltView;
        public override AView OnCreateView(LayoutInflater inflater,
            ViewGroup container,
            Bundle savedInstanceState) => currentBuiltView = view.ToView(false);
        public override void OnDestroy()
        {
            if(view  != null)
            {
                view.ViewHandler = null;
            }
            if (currentBuiltView != null)
            {
                currentBuiltView?.Dispose();
                currentBuiltView = null;
            }
            base.OnDestroy();
            this.Dispose();
        }
    }
}
