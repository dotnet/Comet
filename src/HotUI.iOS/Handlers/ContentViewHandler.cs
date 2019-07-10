using System;
using System.Collections.Generic;
using HotUI.iOS.Controls;
using UIKit;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.iOS
{
    public class ContentViewHandler : iOSViewHandler
    {
        private UIView _view;
        private ContentView _contentView;

        public UIView View => _view;

        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;
        public event EventHandler RemovedFromView;

        public object NativeView => View;

        public HUIContainerView ContainerView => null;
        
        public bool HasContainer
        {
            get => false;
            set { }
        } 
        public void Remove(View view)
        {
            _view = null;
            _contentView = null;
        }

        public void SetView(View view)
        {
            _contentView = view as ContentView;
            _view = _contentView?.Content?.ToView();
        }

        public void UpdateValue(string property, object value)
        {
        }
    }
}