using System;
using Comet.Android.Controls;
using AView = Android.Views.View;

namespace Comet.Android
{
    public interface AndroidViewHandler : IViewHandler
    {
        event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        AView View { get; }

        HUITouchGestureListener GestureListener { get; set; }
    }
}