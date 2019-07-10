using System;
using System.Diagnostics;
using CoreAnimation;
using CoreGraphics;
using HotUI.Drawing;
using HotUI.iOS.Controls;
using UIKit;
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS
{
    public class ViewHandler : iOSViewHandler
    {
        public static readonly PropertyMapper<View> Mapper = new PropertyMapper<View>()
        {
            [nameof(EnvironmentKeys.Colors.BackgroundColor)] = MapBackgroundColorProperty,
            [nameof(EnvironmentKeys.View.Shadow)] = MapShadowProperty,
            [nameof(EnvironmentKeys.View.ClipShape)] = MapClipShapeProperty
        };
        
        private View _view;
        private UIView _body;

        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;
        public event EventHandler RemovedFromView;

        public UIView View => _body;
        
        public HUIContainerView ContainerView => null;

        public object NativeView => View;

        public bool HasContainer { get; set; } = false;

        public void Remove(View view)
        {
            _view = null;
            _body = null;
        }

        public void SetView(View view)
        {
            var oldBody = _body;
            _view = view;
            SetBody();
            Mapper.UpdateProperties(this, _view);
            NativeViewChanged?.Invoke(this, new ViewChangedEventArgs(_view, oldBody, _body));
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _view, property);
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

        public static bool MapBackgroundColorProperty(IViewHandler handler, View virtualView)
        {
            var nativeView = (UIView) handler.NativeView;
            var color = virtualView.GetBackgroundColor();
            if (color != null)
                nativeView.BackgroundColor = color.ToUIColor();
            return true;
        }
        
        public static bool MapShadowProperty(IViewHandler handler, View virtualView)
        {
            var nativeView = (UIView) handler.NativeView;
            var shadow = virtualView.GetShadow();
            var clipShape = virtualView.GetClipShape();

            // If there is a clip shape, then the shadow should be applied to the clip layer, not the view layer
            if (shadow != null && clipShape == null)
            {
                handler.HasContainer = false;
                ApplyShadowToLayer(shadow, nativeView.Layer);
            }

            return true;
        }

        private static void ApplyShadowToLayer(Shadow shadow, CALayer layer)
        {
            layer.ShadowColor = shadow.Color.ToCGColor();
            layer.ShadowRadius = (nfloat) shadow.Radius;
            layer.ShadowOffset = shadow.Offset.ToCGSize();
            layer.ShadowOpacity = shadow.Opacity;
        }

        public static bool MapClipShapeProperty(IViewHandler handler, View virtualView)
        {
            var nativeView = (UIView) handler.NativeView;
            var clipShape = virtualView.GetClipShape();
            if (clipShape != null)
            {
                handler.HasContainer = true;
                var bounds = nativeView.Bounds;
                
                var layer = new CAShapeLayer
                {
                    Frame = bounds
                };

                if (clipShape is Circle)
                {
                    var size = Math.Min(bounds.Width, bounds.Height);
                    var x = (bounds.Width - size) / 2;
                    var y = (bounds.Height - size) / 2;
                    
                    var path = new CGPath();
                    path.AddEllipseInRect(new CGRect(x,y,size,size));
                    path.CloseSubpath();
                    
                    layer.Path = path;
                }
                else if (clipShape is Path)
                {
                    
                }

                var viewHandler = handler as iOSViewHandler;
                if (viewHandler?.ContainerView != null)
                    viewHandler.ContainerView.MaskLayer = layer;

                var shadow = virtualView.GetShadow();
                if (shadow != null)
                {
                    var shadowLayer = new CAShapeLayer();
                    shadowLayer.Name = "shadow";
                    shadowLayer.FillColor = new CGColor(0,0,0,1);
                    shadowLayer.Path = layer.Path;
                    shadowLayer.Frame = layer.Frame;
        
                    ApplyShadowToLayer(shadow, shadowLayer);
                    
                    if (viewHandler?.ContainerView != null)
                        viewHandler.ContainerView.ShadowLayer = shadowLayer;
                }
            }
            else
            {
                nativeView.Layer.Mask = null;
                handler.HasContainer = false;
            }

            return true;
        }
    }
}