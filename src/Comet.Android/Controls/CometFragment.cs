using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using AView = Android.Views.View;

namespace Comet.Android.Controls
{
    public class CometFragment : Fragment
    {
        private CometView _containerView;
        private View _startingCurrentView;
        
        public CometFragment()
        {
        }

        public CometFragment(View view)
        {
            this.CurrentView = view;
        }

        public string Title { get; set; }

        public View CurrentView
        {
            get => _containerView?.CurrentView ?? _startingCurrentView;
            set
            {
                if (_containerView != null)
                    _containerView.CurrentView = value;
                else
                    _startingCurrentView = value;

                Title = value?.GetEnvironment<string>(EnvironmentKeys.View.Title) ?? value?.BuiltView?.GetEnvironment<string>(EnvironmentKeys.View.Title) ?? "";
            }
        }

        public override AView OnCreateView(LayoutInflater inflater,
            ViewGroup container,
            Bundle savedInstanceState)
        {
            _containerView = new CometView(AndroidContext.CurrentContext);
            _containerView.CurrentView = _startingCurrentView;
            _startingCurrentView = null;
            return _containerView;
        }

        public override void OnDestroy()
        {
            if (_containerView != null)
            {
                _containerView.CurrentView.ViewHandler = null;
                _containerView.CurrentView?.Dispose();
                _containerView.CurrentView = null;
            }
            base.OnDestroy();
            this.Dispose();
        }
    }
}
