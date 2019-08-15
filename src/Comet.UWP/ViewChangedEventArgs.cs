using System;
using Windows.UI.Xaml;

namespace Comet.UWP
{
    public class ViewChangedEventArgs : EventArgs
    {
        public View VirtualView { get; }
        public UIElement OldNativeView { get; }
        public UIElement NewNativeView { get; }

        public ViewChangedEventArgs(
            View view,
            UIElement oldNativeView,
            UIElement newNativeView)
        {
            VirtualView = view;
            OldNativeView = oldNativeView;
            NewNativeView = newNativeView;
        }
    }
}
