using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using AView = Android.Views.View;
using HotUI.Android.Controls;
using HotUI.Android.Handlers;
namespace HotUI.Android
{
    public abstract class HotUIActivity : AppCompatActivity
    {
        private View _page;

        public View Page
        {
            get => _page;
            set
            {
                _page = value;
                SetView(_page,false,false);
            }
        }
        CustomFrameLayout mainFrame;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AndroidContext.CurrentContext = this;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            mainFrame = new CustomFrameLayout(this);
            SetContentView(mainFrame);
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void SetView(View view,bool animate, bool isNavigate)
        {
            if(!isNavigate && this.SupportFragmentManager.BackStackEntryCount > 0)
            {
                this.SupportFragmentManager.PopBackStack(0, (int)PopBackStackFlags.Inclusive);
            }
           
            var fragment = new HotUIFragment(view);
            view.ToView();
            if (view.BuiltView is NavigationView nav)
            {
                nav.PerformNavigate = Navigate;
            }
            var transaction = this.SupportFragmentManager.BeginTransaction();
           
            if (animate)
                transaction.SetTransition((int)global::Android.App.FragmentTransit.FragmentFade);
            if(isNavigate)
                transaction.AddToBackStack(view.Id);
            transaction.Replace(mainFrame.Id, fragment);
            transaction.CommitAllowingStateLoss();
            
        }

        public void Navigate(View view) => SetView(view, true, true);

        public void NavigatePop()
        {

        }

        public void PresentModal(View view)
        {
            //var frag = new HotUIFragment(view);
            //DialogFragment
        }
        public void DismissModal()
        {

        }
        //public override void OnBackPressed()
        //{
        //    var manager = SupportFragmentManager;
        //    if (manager.BackStackEntryCount > 1)
        //        manager.PopBackStack();
        //    else
        //        base.OnBackPressed();
        //}
    }
}