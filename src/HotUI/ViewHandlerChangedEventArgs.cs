using System;
namespace HotUI
{
    public class ViewHandlerChangedEventArgs : EventArgs
    {
        public View VirtualView { get; set; }
        public IViewHandler OldViewHandler { get; }
        public IViewHandler NewViewHandler { get; }

        public ViewHandlerChangedEventArgs(
            View view,
            IViewHandler oldNativeView,
            IViewHandler newNativeView)
        {
            VirtualView = view;
            OldViewHandler = oldNativeView;
            NewViewHandler = newNativeView;
        }
    }
}
