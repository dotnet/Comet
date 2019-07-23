using System;
using HotUI.Android.Controls;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public interface AndroidViewHandler : IViewHandler
    {
        event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        AView View { get; }

        HUITouchGestureListener GestureListener { get; set; }
    }
}