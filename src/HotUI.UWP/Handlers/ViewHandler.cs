using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.UWP.Handlers
{
    public class ViewHandler : Grid, IUIElement
    {
        private static readonly PropertyMapper<View, ViewHandler> Mapper = new PropertyMapper<View, ViewHandler>()
        {
            [nameof(global::HotUI.View.Body)] = MapBodyProperty
        };
        
        private View _view;
        private UIElement _body;
        
        public Action ViewChanged { get; set; }

        public UIElement View => _body;

        public void Remove(View view)
        {
            _view = null;
            _body = null;
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

        public static bool MapBodyProperty(ViewHandler nativeView, View virtualView)
        {
            var uiElement = virtualView?.ToIUIElement();
            if (uiElement?.GetType() == typeof(ViewHandler) && virtualView.Body == null)
            {
                Debug.WriteLine($"There is no ViewHandler for {virtualView.GetType()}");
                return true;
            }

            nativeView._body = uiElement?.View;
            return true;
        }
    }
}