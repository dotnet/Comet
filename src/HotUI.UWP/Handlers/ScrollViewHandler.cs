using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using UWPScrollView = Windows.UI.Xaml.Controls.ScrollViewer;

namespace HotUI.UWP
{
    public class ScrollViewHandler : IUIElement
    {
        private static readonly PropertyMapper<ScrollView, UWPScrollView> Mapper = new PropertyMapper<ScrollView, UWPScrollView>(
            new Dictionary<string, Func<UWPScrollView, ScrollView, bool>>()
            {
            });

        private ScrollView _virtualScrollView;
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
            Mapper.UpdateProperties(_nativeScrollView, _virtualScrollView);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(_nativeScrollView, _virtualScrollView, property);
        }
    }
}