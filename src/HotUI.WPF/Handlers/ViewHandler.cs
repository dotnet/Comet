using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HotUI.WPF
{
    public class ViewHandler : IUIElement
    {
        private static readonly PropertyMapper<View, ViewHandler> Mapper = new PropertyMapper<View, ViewHandler>(new Dictionary<string, Func<ViewHandler, View, bool>>()
        {
            
        });
        
        private View _view;

        public Action ViewChanged { get; set; }

        public UIElement View => _view?.ToView() ?? new Canvas();

        public void Remove(View view)
        {
            _view = null;
        }

        public void SetView(View view)
        {
            _view = view;
            Mapper.UpdateProperties(this, _view);
            ViewChanged?.Invoke();
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperties(this, _view);
        }
    }
}