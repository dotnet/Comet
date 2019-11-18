using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using AView = Android.Views.View;

namespace Comet.Android.Controls
{
    public class CometFragment : Fragment
    {
        private readonly View view;
        public string Title { get; }

        public CometFragment()
        {
        }

        public CometFragment(View view)
        {
            this.view = view;
            this.Title = view?.GetEnvironment<string>(EnvironmentKeys.View.Title) ?? "";
        }

        AView currentBuiltView;
        public override AView OnCreateView(LayoutInflater inflater,
            ViewGroup container,
            Bundle savedInstanceState) => currentBuiltView = view.ToView();

        public override void OnDestroy()
        {
            if (view != null)
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
