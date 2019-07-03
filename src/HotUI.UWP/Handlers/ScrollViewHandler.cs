using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using UWPScrollView = Windows.UI.Xaml.Controls.ScrollViewer;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.UWP.Handlers
{
    public class ScrollViewHandler : IUIElement
    {
        private static readonly PropertyMapper<ScrollView, UWPScrollView> Mapper = new PropertyMapper<ScrollView, UWPScrollView>()
            {
            };

        private ScrollView _virtualScrollView;
        private UIElement _content;
        private readonly UWPScrollView _nativeScrollView;
        
        public ScrollViewHandler()
        {
            _nativeScrollView = new UWPScrollView();
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