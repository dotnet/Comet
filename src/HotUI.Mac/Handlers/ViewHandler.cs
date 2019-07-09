using System;
using System.Diagnostics;
using AppKit;
using CoreAnimation;
using CoreGraphics;
using HotUI.Drawing;
using HotUI.Mac.Controls;
using HotUI.Mac.Extensions;

namespace HotUI.Mac
{
	public class ViewHandler : MacViewHandler
    {
        public static readonly PropertyMapper<View> Mapper = new PropertyMapper<View>()
        {
            [nameof(EnvironmentKeys.Colors.BackgroundColor)] = MapBackgroundColorProperty,
            [nameof(EnvironmentKeys.View.Shadow)] = MapShadowProperty,
            [nameof(EnvironmentKeys.View.ClipShape)] = MapClipShapeProperty
            
        };

        private View _view;
        private NSView _body;

        public Action ViewChanged { get; set; }

        public NSView View => _body;

        public object NativeView => View;
        public bool HasContainer { get; set; } = false;
        public HUIContainerView ContainerView => null;

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
            Mapper.UpdateProperty(this, _view, property);
        }

        public bool SetBody()
        {
            var uiElement = _view?.ToINSView();
            if (uiElement?.GetType() == typeof(ViewHandler) && _view.Body == null)
            {
                // this is recursive.
                Debug.WriteLine($"There is no ViewHandler for {_view.GetType()}");
                return true;
            }

            _body = uiElement?.View ?? new NSColorView();
            return true;
        }

        public static bool MapBackgroundColorProperty(IViewHandler viewHandler, View virtualView)
        {
            var nativeView = (NSView)viewHandler.NativeView;
            var color = virtualView.GetBackgroundColor();
            if (color != null)
            {
                if (nativeView is NSColorView colorView)
                {
                    colorView.BackgroundColor = color.ToNSColor();
                }
                else if (nativeView is NSTextField textField)
                {
                    textField.BackgroundColor = color.ToNSColor();
                    textField.DrawsBackground = true;
                }
            }

            return true;
        }
        
        public static bool MapShadowProperty(IViewHandler handler, View virtualView)
        {
            var nativeView = (NSView) handler.NativeView;
            var shadow = virtualView.GetShadow();
            var clipShape = virtualView.GetClipShape();

            // If there is a clip shape, then the shadow should be applied to the clip layer, not the view layer
            if (shadow != null && clipShape == null)
            {
                handler.HasContainer = false;
                if (nativeView.Layer == null)
                    nativeView.WantsLayer = true;
                
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
            var nativeView = (NSView) handler.NativeView;
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

                var viewHandler = handler as MacViewHandler;
                if (viewHandler?.ContainerView != null)
                    viewHandler.ContainerView.MaskLayer = layer;

                var shadow = virtualView.GetShadow();
                if (shadow != null)
                {
                    var shadowLayer = new CAShapeLayer();
                    shadowLayer.Name = "shadow";
                    shadowLayer.FillColor = new CGColor(0,0,0,0);
                    shadowLayer.Path = layer.Path;
                    shadowLayer.Frame = layer.Frame;
        
                    ApplyShadowToLayer(shadow, shadowLayer);
                    
                    if (viewHandler?.ContainerView != null)
                        viewHandler.ContainerView.ShadowLayer = shadowLayer;
                }
            }
            else
            {
                handler.HasContainer = false;
            }

            return true;
        }
    }
}
