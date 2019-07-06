using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.UWP.Handlers
{
    public class ViewHandler : IUIElement
    {
        public static readonly PropertyMapper<View, UIElement, ViewHandler> Mapper = new PropertyMapper<View, UIElement, ViewHandler>()
        {

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
			SetBody();
            Mapper.UpdateProperties(this, _view);
            ViewChanged?.Invoke();
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperties(this, _view);
        }

        public void SetBody()
        {
            var uiElement = _view?.ToIUIElement();
            if (uiElement?.GetType() == typeof(ViewHandler) && _view.Body == null)
            {
                Debug.WriteLine($"There is no ViewHandler for {_view.GetType()}");
            }

            _body = uiElement?.View;
        }
    }
}