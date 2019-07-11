using System;
using FView = Xamarin.Forms.View;

namespace HotUI.Forms
{
    public class ViewChangedEventArgs : EventArgs
    {
        public View VirtualView { get; }
        public FView OldNativeView { get; }
        public FView NewNativeView { get; }

        public ViewChangedEventArgs(
            View view,
            FView oldNativeView,
            FView newNativeView)
        {
            VirtualView = view;
            OldNativeView = oldNativeView;
            NewNativeView = newNativeView;
        }
    }
}
