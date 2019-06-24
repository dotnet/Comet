using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public abstract class HotActivity : AppCompatActivity
    {
        private HotPage _page;

        public HotPage Page
        {
            get => _page;
            set
            {
                _page = value;
                SetContentView(_page?.ToView(this));
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AndroidContext.CurrentContext = this;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        protected override void OnRestart()
        {
            base.OnRestart();
            _page?.OnAppearing();
        }

        protected override void OnResume()
        {
            base.OnResume();
            _page?.OnAppearing();
        }

        protected override void OnStart()
        {
            base.OnStart();
            _page?.OnAppearing();
        }

        protected override void OnStop()
        {
            base.OnStop();
            _page?.OnDisppearing();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _page?.OnDisppearing();
        }
    }

    public static partial class ControlExtensions
    {
        public static void UpdateProperties(this Activity view, HotPage hView)
        {
            view.Title = hView.Title;
        }

        public static bool UpdateProperty(this Activity view, string property, object value)
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