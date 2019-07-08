using System;
using System.Diagnostics;
using CoreAnimation;
using CoreGraphics;
using HotUI.Drawing;
using UIKit;
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS
{
    public class ViewHandler : IUIView
    {
        public static readonly PropertyMapper<View, UIView, UIView> Mapper = new PropertyMapper<View, UIView, UIView>()
        {
            [nameof(EnvironmentKeys.Colors.BackgroundColor)] = MapBackgroundColorProperty,
            [nameof(EnvironmentKeys.View.Shadow)] = MapShadowRadiusProperty,
            [nameof(EnvironmentKeys.View.ClipShape)] = MapClipShapeProperty
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
            var clipShape = virtualView.GetClipShape();

            // If there is a clip shape, then the shadow should be applied to the clip layer, not the view layer
            if (shadow != null && clipShape == null)
                ApplyShadowToLayer(shadow, nativeView.Layer);

            return true;
        }

        private static void ApplyShadowToLayer(Shadow shadow, CALayer layer)
        {
            layer.ShadowColor = shadow.Color.ToCGColor();
            layer.ShadowRadius = (nfloat) shadow.Radius;
            layer.ShadowOffset = shadow.Offset.ToCGSize();
            layer.ShadowOpacity = shadow.Opacity;
        }

        public static bool MapClipShapeProperty(UIView nativeView, View virtualView)
        {
            var clipShape = virtualView.GetClipShape();
            if (clipShape != null)
            {
                var bounds = nativeView.Bounds;
                
                var layer = new CAShapeLayer
                {
                    Bounds = bounds
                };

                if (clipShape is Circle)
                {
                    var size = Math.Min(bounds.Width, bounds.Height);
                    var x = (bounds.Width - size) / 2;
                    var y = (bounds.Height - size) / 2;
                    
                    var path = new CGPath();
                    path.AddEllipseInRect(new CGRect(x,y,size,size));

                    layer.Path = path;
                }
                else if (clipShape is Path)
                {
                    
                }
                
                nativeView.Layer.Mask = layer;
                var shadow = virtualView.GetShadow();
                if (shadow != null)
                {
                    var shadowLayer = new CAShapeLayer();
                    shadowLayer.FillColor = new CGColor(0,0,0,0);
                    shadowLayer.Path = layer.Path;
                    shadowLayer.Frame = layer.Frame;
        
                    ApplyShadowToLayer(shadow, shadowLayer);

                    nativeView.Superview?.Layer.InsertSublayerBelow(shadowLayer, nativeView.Layer);
                }
            }

            return true;
        }
    }
}