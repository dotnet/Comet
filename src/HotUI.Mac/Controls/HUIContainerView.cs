using System;
using CoreAnimation;
using CoreGraphics;
using AppKit;

namespace HotUI.Mac.Controls
{
    public class HUIContainerView : NSView
    {
        private NSView _mainView;
        private CAShapeLayer _shadowLayer;
        private CAShapeLayer _maskLayer;
        private CGSize _size;
        
        public HUIContainerView()
        {
            AutoresizesSubviews = true;
        }

        public override CGRect Frame
        {
            get => base.Frame;
            set
            {
                base.Frame = value;
                
                if (_shadowLayer != null || _maskLayer != null)
                {
                    if (!_size.Equals(value.Size))
                    {
                        var fx = value.Size.Width / _size.Width;
                        var fy = value.Size.Height / _size.Height;
                        var transform = CGAffineTransform.MakeScale(fx, fy);
                        var path = _shadowLayer?.Path ?? _maskLayer?.Path;
                        var transformedPath = path.CopyByTransformingPath(transform);
                        if (_shadowLayer != null)
                            _shadowLayer.Path = transformedPath;

                        if (_maskLayer != null)
                            _maskLayer.Path = transformedPath;
                    }
                }

                _size = value.Size;
            }
        }

        public NSView MainView
        {
            get => _mainView;
            set
            {
                if (_mainView != null)
                {
                    ShadowLayer = null;
                    MaskLayer = null;
                    _mainView.RemoveFromSuperview();
                }

                _mainView = value;

                if (_mainView != null)
                {
                    base.Frame = _mainView.Bounds;
                    _size = _mainView.Bounds.Size;
                    _mainView.AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable;
                    _mainView.Frame = Bounds;
                    AddSubview(_mainView);
                }
            }
        }
        
        public CAShapeLayer ShadowLayer
        {
            get => _shadowLayer;
            set
            {
                _shadowLayer?.RemoveFromSuperLayer();
                _shadowLayer = value;
                if (_shadowLayer != null && _mainView != null)
                {
                    if (Layer == null)
                        WantsLayer = true;
                    
                    Layer.InsertSublayerBelow(_shadowLayer, _mainView.Layer);
                }
            }
        }
        
        public CAShapeLayer MaskLayer
        {
            get => _maskLayer;
            set
            {
                _maskLayer = value;
                if (_mainView != null)
                {
                    if (_mainView.Layer == null)
                        _mainView.WantsLayer = true;
                    
                    _mainView.Layer.Mask = value;
                }
            }
        }
    }
}