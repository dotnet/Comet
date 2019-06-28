using System;
using System.Collections.Generic;
using System.Diagnostics;
using UIKit;
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS
{
    public class ViewHandler : IUIView
    {
        private static readonly PropertyMapper<View, ViewHandler> Mapper = new PropertyMapper<View, ViewHandler>()
        {
            [nameof(HotUI.View.Body)] = MapBodyProperty
        };
        
        private View _view;
        private UIView _body;
        
        public Action ViewChanged { get; set; }

        public UIView View => _body;
        
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
            var uiElement = virtualView?.ToIUIView();
            if (uiElement?.GetType() == typeof(ViewHandler) && virtualView.Body == null)
            {
                // this is recursive.
                Debug.WriteLine($"There is no ViewHandler for {virtualView.GetType()}");
                return true;
            }

            nativeView._body = uiElement?.View ?? new UIView();
            return true;
        }
    }
}