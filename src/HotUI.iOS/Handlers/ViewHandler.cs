using System;
using System.Diagnostics;
using UIKit;
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS
{
    public class ViewHandler : IUIView
    {
        public static readonly PropertyMapper<View, UIView, UIView> Mapper = new PropertyMapper<View, UIView, UIView>()
        {
            [nameof(EnvironmentKeys.Colors.BackgroundColor)] = MapBackgroundColorProperty,
            [nameof(EnvironmentKeys.View.Shadow)] = MapShadowRadiusProperty
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
            SetBody();
            Mapper.UpdateProperties(_body, _view);
            ViewChanged?.Invoke();
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(_body, _view, property);
        }

        public bool SetBody()
        {
            var uiElement = _view?.ToIUIView();
            if (uiElement?.GetType() == typeof(ViewHandler) && _view.Body == null)
            {
                // this is recursive.
                Debug.WriteLine($"There is no ViewHandler for {_view.GetType()}");
                return true;
            }

            _body = uiElement?.View ?? new UIView();
            return true;
        }

        public static bool MapBackgroundColorProperty(UIView nativeView, View virtualView)
        {
            var color = virtualView.GetBackgroundColor();
            if (color != null)
                nativeView.BackgroundColor = color.ToUIColor();
            return true;
        }
        
        public static bool MapShadowRadiusProperty(UIView nativeView, View virtualView)
        {
            var shadow = virtualView.GetShadow();
            if (shadow != null)
            {
                nativeView.Layer.ShadowColor = shadow.Color.ToCGColor();
                nativeView.Layer.ShadowRadius = (nfloat)shadow.Radius;
                nativeView.Layer.ShadowOffset = shadow.Offset.ToCGSize();
                nativeView.Layer.ShadowOpacity = shadow.Opacity;
            }

            return true;
        }
    }
}