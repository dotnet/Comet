using System;
using Android.Support.V4.App;
using PopBackStackFlags = Android.App.PopBackStackFlags;

namespace HotUI.Android.Controls
{
    public class NavigationViewWrapper : CustomFrameLayout
    {
        public NavigationViewWrapper(View view) : base(AndroidContext.CurrentContext)
        {
            SetRoot(view);
        }

        public void SetRoot(View view) => SetView(view, false, false);

        void Navigate(View view) => Push(view);

        public void Push(View view, bool animated = true) => SetView(view, animated, true);

        FragmentManager FragmentManager => AndroidContext.AppCompatActivity.SupportFragmentManager;

        void SetView(View view, bool animate, bool isNavigate)
        {
            var manager = FragmentManager;
            if (!isNavigate && manager.BackStackEntryCount > 0)
            {
                manager.PopBackStack(0, (int)PopBackStackFlags.Inclusive);
            }

            var fragment = new HotUIFragment(view);
            view.ToView(false);
            if (view.BuiltView is NavigationView nav)
            {
                nav.PerformNavigate = Navigate;
            }
            var transaction = manager.BeginTransaction();

            if (animate)
                transaction.SetTransition((int)global::Android.App.FragmentTransit.FragmentFade);
            if (isNavigate)
                transaction.AddToBackStack(view.Id);
            transaction.Replace(this.Id, fragment);
            transaction.CommitAllowingStateLoss();

        }
    }
}
