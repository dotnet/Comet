using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public abstract class HotActivity : AppCompatActivity
    {
        private View _page;

        public View Page
        {
            get => _page;
            set
            {
                _page = value;
                SetContentView(_page?.ToView());
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
    }
}