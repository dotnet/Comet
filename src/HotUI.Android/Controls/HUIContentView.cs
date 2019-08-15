using System;
using AView = global::Android.Views.View;
using Comet.Android.Controls;
using LP = Android.Views.ViewGroup.LayoutParams;

namespace Comet.Android.Handlers
{
    public class HUIContentView : CustomFrameLayout
    {
        public HUIContentView() : base(AndroidContext.CurrentContext)
        {
        }
    }
}