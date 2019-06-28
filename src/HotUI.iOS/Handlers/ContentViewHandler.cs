using System;
using System.Collections.Generic;
using UIKit;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.iOS
{
    public class ContentViewHandler : IUIView
    {
        private ContentView _contentView;

        public UIView View => _contentView?.Content?.ToView();

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