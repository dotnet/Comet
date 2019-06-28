using System;
using System.Collections.Generic;
using UIKit;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.iOS
{
    public class ScrollViewHandler : UIScrollView, IUIView
    {
        private static readonly PropertyMapper<ScrollView, ScrollViewHandler> Mapper = new PropertyMapper<ScrollView, ScrollViewHandler>(new Dictionary<string, Func<ScrollViewHandler, ScrollView, bool>>
        {
            
        });

        private ScrollView _scroll;
        private UIView _content;

        public ScrollViewHandler()
        {
            ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Always;
        }

        public UIView View => this;

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
                Add(_content);
                NSLayoutConstraint.ActivateConstraints(new[]
                {
                    _content.LeadingAnchor.ConstraintEqualTo(LeadingAnchor),
                    _content.TrailingAnchor.ConstraintEqualTo(TrailingAnchor),
                    _content.TopAnchor.ConstraintEqualTo(TopAnchor),
                    _content.BottomAnchor.ConstraintEqualTo(BottomAnchor)
                });
            }

            Mapper.UpdateProperties(this, _scroll);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _scroll, property);
        }
    }
}