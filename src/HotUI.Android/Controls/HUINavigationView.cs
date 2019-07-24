using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace HotUI.Android.Controls
{
    public class HUINavigationView : CustomFrameLayout
    {
        public HUINavigationView(Context context) : base(context)
        {
        }

        public void SetRoot(View view)
        {
            AndroidContext.AppCompatActivity.SupportFragmentManager
                .BeginTransaction()
                .Replace(Id, new HotUIFragment(view))
                .CommitAllowingStateLoss();
        }

        public void NavigateTo(View view)
        {
            AndroidContext.AppCompatActivity.SupportFragmentManager.BeginTransaction()
                .SetTransition((int)global::Android.App.FragmentTransit.FragmentFade)
                .AddToBackStack(view.Id)
                .Replace(Id, new HotUIFragment(view))
                .CommitAllowingStateLoss();
        }
    }
}
