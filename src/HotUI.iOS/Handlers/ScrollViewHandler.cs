using System;
using System.Collections.Generic;
using HotUI.iOS.Controls;
using UIKit;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.iOS
{
    public class ScrollViewHandler : UIScrollView, iOSViewHandler
    {
        public static readonly PropertyMapper<ScrollView> Mapper = new PropertyMapper<ScrollView>(ViewHandler.Mapper)
        {
            
        };

        private ScrollView _scroll;
        private UIView _content;

        public ScrollViewHandler()
        {
            ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Always;
        }

        public UIView View => this;

        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        public HUIContainerView ContainerView => null;

        public object NativeView => View;

        public bool HasContainer { get; set; } = false;

        public void Remove(View view)
        {
            _content?.RemoveFromSuperview();
            _scroll = null;
            _content = null;
        }

        public void SetView(View view)
        {
            _scroll = view as ScrollView;

            _content = _scroll?.View?.ToView();
            if (_content != null)
            {
                _content.SizeToFit();
                Add(_content);
            }

            Mapper.UpdateProperties(this, _scroll);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _scroll, property);
        }
    }
}