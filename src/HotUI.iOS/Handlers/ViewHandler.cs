using System;
using System.Diagnostics;
using CoreAnimation;
using CoreGraphics;
using HotUI.Graphics;
using UIKit;

// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS.Handlers
{
    public class ViewHandler : AbstractHandler<View, UIView>
    {
        public static readonly PropertyMapper<View> Mapper = new PropertyMapper<View>()
        {
            [nameof(EnvironmentKeys.Colors.BackgroundColor)] = MapBackgroundColorProperty,
            [nameof(EnvironmentKeys.View.Shadow)] = MapShadowProperty,
            [nameof(EnvironmentKeys.View.ClipShape)] = MapClipShapeProperty,
            [nameof(EnvironmentKeys.View.Overlay)] = MapOverlayProperty
        };
        
        protected override UIView CreateView()
        {
            var viewHandler = VirtualView?.GetOrCreateViewHandler();
            if (viewHandler?.GetType() == typeof(ViewHandler) && NativeView == null)
            {
                // this is recursive.
                Debug.WriteLine($"There is no ViewHandler for {VirtualView.GetType()}");
                return null;
            }

            return viewHandler?.View ?? new UIView();
        }

        public override void SetView(View view)
        {
            var previousView = TypedNativeView;
            base.SetView(view);
            BroadcastNativeViewChanged(previousView, TypedNativeView);
        }

        public static void MapBackgroundColorProperty(IViewHandler handler, View virtualView)
        {
            var nativeView = (UIView) handler.NativeView;
            var color = virtualView.GetBackgroundColor();
            if (color != null)
                nativeView.BackgroundColor = color.ToUIColor();
        }

        public static bool NeedsContainer(View virtualView)
        {
            var overlay = virtualView.GetOverlay();
            if (overlay != null)
                return true;

            var mask = virtualView.GetClipShape();
            if (mask != null)
                return true;

            return false;
        }

        public static void MapShadowProperty(IViewHandler handler, View virtualView)
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
            else
            {
                ClearShadowFromLayer(nativeView.Layer);
                handler.HasContainer = NeedsContainer(virtualView);
            }
        }

        public static void MapOverlayProperty(IViewHandler handler, View virtualView)
        {
            var nativeView = (UIView)handler.NativeView;
            var overlay = virtualView.GetOverlay();

            var viewHandler = handler as iOSViewHandler;


            // If there is a clip shape, then the shadow should be applied to the clip layer, not the view layer
            if (overlay != null)
            {
                handler.HasContainer = true;
                if (viewHandler?.ContainerView != null)
                    viewHandler.ContainerView.OverlayView = overlay.ToView();
            }
            else
            {
                if (viewHandler?.ContainerView != null)
                    viewHandler.ContainerView.OverlayView = null;
                handler.HasContainer = NeedsContainer(virtualView);
            }
        }

        private static void ApplyShadowToLayer(Shadow shadow, CALayer layer)
        {
            layer.ShadowColor = shadow.Color.ToCGColor();
            layer.ShadowRadius = (nfloat) shadow.Radius;
            layer.ShadowOffset = shadow.Offset.ToCGSize();
            layer.ShadowOpacity = shadow.Opacity;
        }

        private static void ClearShadowFromLayer(CALayer layer)
        {
            layer.ShadowColor = new CGColor(0,0,0,0);
            layer.ShadowRadius = 0;
            layer.ShadowOffset = new CGSize();
            layer.ShadowOpacity = 0;
        }

        public static void MapClipShapeProperty(IViewHandler handler, View virtualView)
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

                var path = clipShape.PathForBounds(bounds.ToRectangleF());
                layer.Path = path.ToCGPath();
                
                var viewHandler = handler as iOSViewHandler;
                if (viewHandler?.ContainerView != null)
                {
                    viewHandler.ContainerView.MaskLayer = layer;
                    viewHandler.ContainerView.ClipShape = clipShape;
                }

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
                var shadow = virtualView.GetShadow();
                if (shadow == null)
                    ClearShadowFromLayer(nativeView.Layer);
                nativeView.Layer.Mask = null;
                handler.HasContainer = NeedsContainer(virtualView);
            }
        }
    }
}