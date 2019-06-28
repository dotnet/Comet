using System;
using System.Collections.Generic;
using System.Windows;
using WPFScrollView = System.Windows.Controls.ScrollViewer;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.WPF.Handlers
{
    public class ScrollViewHandler : WPFScrollView, IUIElement
    {
        private static readonly PropertyMapper<ScrollView, WPFScrollView> Mapper = new PropertyMapper<ScrollView, WPFScrollView>()
        {
        };
        
        private ScrollView _scrollView;

        public UIElement View => this;
        
        public void Remove(View view)
        {
        }

        public void SetView(View view)
        {
            _scrollView = view as ScrollView;
            Mapper.UpdateProperties(this, _scrollView);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _scrollView, property);
        }
    }
}