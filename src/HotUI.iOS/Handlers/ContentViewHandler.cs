using System;
using System.Collections.Generic;
using HotUI.iOS.Controls;
using UIKit;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.iOS
{
    public class ContentViewHandler : iOSViewHandler
    {
        private ContentView _contentView;

        public UIView View => _contentView?.Content?.ToView();
        
        public HUIContainerView ContainerView => null;

        public object NativeView => View;

        public bool HasContainer { get; set; } = false;

        public void Remove(View view)
        {
            _contentView = null;
        }

        public void SetView(View view)
        {
            _contentView = view as ContentView;
        }

        public void UpdateValue(string property, object value)
        {
        }
    }
}