using System;
using System.Collections.Generic;
using System.Windows;
using WPFScrollView = System.Windows.Controls.ScrollViewer;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.WPF.Handlers
{
    public class ScrollViewHandler : IUIElement
    {
        public static readonly PropertyMapper<ScrollView, UIElement, WPFScrollView> Mapper = new PropertyMapper<ScrollView, UIElement, WPFScrollView>()
            {
            };

        private ScrollView _virtualScrollView;
        private UIElement _content;
        private readonly WPFScrollView _nativeScrollView;
        
        public ScrollViewHandler()
        {
            _nativeScrollView = new WPFScrollView();
        }
        
        public UIElement View => _nativeScrollView;

        public void Remove(View view)
        {
        }

        public void SetView(View view)
        {
            _virtualScrollView = view as ScrollView;
            _content = _virtualScrollView?.View?.ToEmbeddableView();
            if (_content != null)
            {
                _nativeScrollView.Content = _content;
            }

            Mapper.UpdateProperties(_nativeScrollView, _virtualScrollView);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(_nativeScrollView, _virtualScrollView, property);
        }
    }
}