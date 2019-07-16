using System;
using System.Diagnostics;
using AppKit;
using CoreAnimation;
using CoreGraphics;
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

        public static void MapBackgroundColorProperty(IViewHandler viewHandler, View virtualView)
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
        }
        
        public static void MapShadowProperty(IViewHandler handler, View virtualView)
        {
            var nativeView = (NSView) handler.NativeView;
            var shadow = virtualView.GetShadow();
            var clipShape = virtualView.GetClipShape();

            // If there is a clip shape, then the shadow should be applied to the clip layer, not the view layer
            if (shadow != null && clipShape == null)
            {
                handler.HasContainer = false;
                nativeView.Shadow = new NSShadow()
                {
                    ShadowColor = shadow.Color.ToNSColor(),
                    ShadowOffset = shadow.Offset.ToCGSize(),
                    ShadowBlurRadius = (nfloat) shadow.Radius
                };
                
                /*if (nativeView.Layer == null)
                    nativeView.WantsLayer = true;
                
                ApplyShadowToLayer(shadow, nativeView.Layer);*/
            }
            else if (nativeView != null)
            {
                // todo: Xamarin.Mac bug, you should be able to set Shadow to null.  Either get them to fix this,
                // or use the Objective-C runtime to set this.
                nativeView.Shadow = new NSShadow()
                {
                    ShadowBlurRadius = 0,
                    ShadowColor = NSColor.Clear,
                    ShadowOffset = new CGSize()
                };
            }
        }
        
        public static void MapClipShapeProperty(IViewHandler handler, View virtualView)
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

                var path = clipShape.PathForBounds(bounds.ToRectangleF());
                layer.Path = path.ToCGPath();
                
                var viewHandler = handler as MacViewHandler;
                if (viewHandler?.ContainerView != null)
                    viewHandler.ContainerView.MaskLayer = layer;

                var shadow = virtualView.GetShadow();
                if (shadow != null)
                {
                    if (viewHandler.ContainerView != null)
                    {
                        viewHandler.ContainerView.Shadow = new NSShadow()
                        {
                            ShadowColor = shadow.Color.ToNSColor(),
                            ShadowOffset = shadow.Offset.ToCGSize(),
                            ShadowBlurRadius = (nfloat) shadow.Radius
                        };
                    }
                }
            }
            else
            {
                handler.HasContainer = false;
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
