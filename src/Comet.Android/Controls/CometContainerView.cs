using System;
using System.Drawing;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Comet.Androids;
using AView = Android.Views.View;
using APath = Android.Graphics.Path;
using Comet.Skia;

namespace Comet.Android.Controls
{
	public class CometContainerView : LinearLayout
	{
		private AView _mainView;
		private Shape clipShape;
		private SkiaShapeView overlayShapeView;
		public View View { get; set; }
		protected CometContainerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public CometContainerView() : base(AndroidContext.CurrentContext)
		{

		}

		public AView MainView
		{
			get => _mainView;
			set
			{
				if (_mainView != null)
				{
					RemoveView(_mainView);
				}

				_mainView = value;

				if (_mainView != null)
				{
					_mainView.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
					AddView(_mainView,0);
				}
			}
		}
		AView skiaViewNative;
		public SkiaShapeView OverlayShapeView
		{
			get => overlayShapeView;
			set
			{
				overlayShapeView = value;
				if (value != null && skiaViewNative == null)
				{
					skiaViewNative = value.ToView();
					skiaViewNative.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
					var index = this.IndexOfChild(_mainView) ;
					AddView(skiaViewNative,index + 1);
				}
				(skiaViewNative as AndroidViewHandler)?.SetView(value);
			}
		}

		public Shape ClipShape
		{
			get => clipShape;
			set
			{
				clipShape = value;
				ApplyShape();
			}
		}
		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			base.OnLayout(changed, l, t, r, b);
			if (changed)
			{
				_mainView?.Layout(l, t, r, b);
				skiaViewNative?.Layout(l, t, r, b);
			}
		}

		void ApplyShape()
		{
			currentPath = null;
			this.InvalidateOutline();
		}

		APath currentPath;
		SizeF lastPathSize;

		protected override void DispatchDraw(Canvas canvas)
		{
			var bounds = new RectangleF(0, 0, Width, Height);
			if (ClipShape != null)
			{
				if (bounds.Size != lastPathSize || currentPath == null)
				{
					var path = ClipShape.PathForBounds(bounds);
					currentPath = path.AsAndroidPath();
					lastPathSize = bounds.Size;
				}
				canvas.ClipPath(currentPath);
			}
			base.DispatchDraw(canvas);		
		}


		//class CometShapeOutlineProvider : ViewOutlineProvider
		//{
		//	public Shape Shape {
		//		get => shape;
		//		set
		//		{
		//			if (shape == value)
		//				return;
		//			shape = value;
		//			currentPath = null;
		//		}
		//	}
		//	RectangleF lastBounds;
		//	APath currentPath;
		//	private Shape shape;

		//	public override void GetOutline(AView view, Outline outline)
		//	{
		//		if (Shape == null)
		//		{
		//			outline.SetEmpty();
		//			return;
		//		}
		//		var bounds = new RectangleF(0, 0, view.Width, view.Height);
		//		if (bounds != lastBounds || currentPath == null)
		//		{
		//			var path = Shape.PathForBounds(bounds);
		//			currentPath = path.AsAndroidPath();
		//			lastBounds = bounds;
		//		}
		//		outline.SetRoundRect(view.ClipBounds, 60);
		//		//outline.SetConvexPath(currentPath);
		//	}
		//}
	}

}
