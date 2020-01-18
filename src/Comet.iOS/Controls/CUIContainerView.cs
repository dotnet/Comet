using System;
using System.Drawing;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace Comet.iOS.Controls
{
	public class CUIContainerView : UIView
	{
		private UIView _mainView;
		private UIView _overlayView;
		private ShapeView _overlayShapeView;
		private CAShapeLayer _shadowLayer;
		private CAShapeLayer _maskLayer;
		private CGSize _size;
		private CGSize _intrinsicContentSize;
		private Shape _clipShape;

		public CUIContainerView()
		{
			AutosizesSubviews = true;
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
						if (_clipShape != null)
						{
							var path = _clipShape.PathForBounds(new RectangleF(0, 0, (float)value.Width, (float)value.Height));
							if (_shadowLayer != null)
								_shadowLayer.Path = path.ToCGPath();

							if (_maskLayer != null)
								_maskLayer.Path = path.ToCGPath();
						}
						else
						{
							var fx = value.Size.Width / _size.Width;
							var fy = value.Size.Height / _size.Height;
							var transform = CGAffineTransform.MakeScale(fx, fy);
							var path = _shadowLayer?.Path ?? _maskLayer?.Path;
							var transformedPath = path?.CopyByTransformingPath(transform);
							if (_shadowLayer != null)
								_shadowLayer.Path = transformedPath;

							if (_maskLayer != null)
								_maskLayer.Path = transformedPath;
						}
					}
				}

				_size = value.Size;
			}
		}

		public UIView MainView
		{
			get => _mainView;
			set
			{
				if (_mainView != null)
				{
					ShadowLayer = null;
					MaskLayer = null;
					ClipShape = null;
					_mainView.RemoveFromSuperview();
				}

				_mainView = value;

				if (_mainView != null)
				{
					base.Frame = _mainView.Bounds;
					_size = _mainView.Bounds.Size;
					_mainView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
					_mainView.Frame = Bounds;

					if (_overlayView != null)
						InsertSubviewBelow(_mainView, _overlayView);
					else
						AddSubview(_mainView);
				}
			}
		}

		public ShapeView OverlayShapeView
		{
			get => _overlayShapeView;
			set{
				_overlayShapeView?.Dispose();
				_overlayShapeView = value;
				OverlayView = value?.ToView();
			}
		}

		UIView OverlayView
		{
			get => _overlayView;
			set
			{
				_overlayView?.RemoveFromSuperview();

				_overlayView = value;

				if (_overlayView != null)
				{
					base.Frame = _mainView?.Bounds ?? Bounds;
					_overlayView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
					_overlayView.Frame = Bounds;
					AddSubview(_overlayView);
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
					Layer.InsertSublayerBelow(_shadowLayer, _mainView.Layer);
			}
		}

		public override CGSize IntrinsicContentSize => _intrinsicContentSize;

		public override CGSize SizeThatFits(CGSize size)
		{
			if (_mainView != null)
			{
				_intrinsicContentSize = _mainView.SizeThatFits(size);
				return _intrinsicContentSize;
			}

			return base.SizeThatFits(size);
		}

		public override void SizeToFit()
		{
			if (_mainView != null)
			{
				_mainView.SizeToFit();
				_intrinsicContentSize = _mainView.Bounds.Size;
				Frame = _mainView.Bounds;
			}
			else
			{
				base.SizeToFit();
			}
		}

		public CAShapeLayer MaskLayer
		{
			get => _maskLayer;
			set
			{
				if (_maskLayer != null)
					_mainView.Layer.Mask = null;

				_maskLayer = value;
				if (_mainView != null)
					_mainView.Layer.Mask = value;
			}
		}

		public Shape ClipShape
		{
			get => _clipShape;
			set => _clipShape = value;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			OverlayShapeView?.Dispose();
		}
	}
}
