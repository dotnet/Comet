using System;
using System.Diagnostics;
using CoreAnimation;
using CoreGraphics;
using HotUI.Graphics;
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
            [nameof(EnvironmentKeys.View.ClipShape)] = MapClipShapeProperty,
            [nameof(EnvironmentKeys.View.Overlay)] = MapOverlayProperty
        };
        
        private View _view;
        private UIView _body;
        
        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;
    
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
                var shadow = virtualView.GetShadow();
                if (shadow == null)
                    ClearShadowFromLayer(nativeView.Layer);
                nativeView.Layer.Mask = null;
                handler.HasContainer = NeedsContainer(virtualView);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
              
            _body?.RemoveFromSuperview();
            _body?.Dispose();
            _body = null;
            if (_view != null)
                Remove(_view);
             
        }


        void OnDispose(bool disposing)
        {
            if (disposedValue)
                return;
            disposedValue = true;
            Dispose(disposing);
        }

        ~ViewHandler()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            OnDispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            OnDispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}