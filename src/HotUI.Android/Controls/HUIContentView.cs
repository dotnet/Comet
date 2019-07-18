using System;
using AView = global::Android.Views.View;
using HotUI.Android.Controls;
using LP = Android.Views.ViewGroup.LayoutParams;

namespace HotUI.Android.Handlers
{
    public class HUIContentView : CustomFrameLayout
    {
        public HUIContentView() : base(AndroidContext.CurrentContext)
        {
        }
    }
}