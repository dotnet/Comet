using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AView = Android.Views.View;

namespace HotUI.Android.Controls
{
    public class HUIContainerView : LinearLayout
    {
        private AView _mainView;
        
        protected HUIContainerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public HUIContainerView() : base(AndroidContext.CurrentContext)
        {
        }

        public AView MainView
        {
            get => _mainView;
            set
            {
                if (_mainView != null)
                {
                    RemoveView(_mainView);
                }

                _mainView = value;

                if (_mainView != null)
                {
                    _mainView.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                    AddView(_mainView);
                }
            }
        }
    }
}