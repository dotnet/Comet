using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using LP = Android.Views.ViewGroup.LayoutParams;
namespace HotUI.Android.Controls
{
    public class CustomFrameLayout : FrameLayout
    {
        public CustomFrameLayout(Context context) : base(context)
        {
            LayoutParameters = new LP(LP.MatchParent, LP.MatchParent);
            Id = global::Android.Views.View.GenerateViewId();
        }

        public CustomFrameLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public CustomFrameLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public CustomFrameLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected CustomFrameLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override WindowInsets OnApplyWindowInsets(WindowInsets insets)
        {
            var leftPadding = PaddingLeft;

            var result = base.OnApplyWindowInsets(insets);

            SetPadding(leftPadding, PaddingTop, PaddingRight, PaddingBottom);

            return result;
        }
    }
}