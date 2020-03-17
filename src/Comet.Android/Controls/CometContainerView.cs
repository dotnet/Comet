using System;
using System.Drawing;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Comet.Androids;
using AView = Android.Views.View;

namespace Comet.Android.Controls
{
	public class CometContainerView : LinearLayout
	{
		private AView _mainView;
		private Shape clipShape;

		protected CometContainerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public CometContainerView() : base(AndroidContext.CurrentContext)
		{
			this.OutlineProvider = new CometShapeOutlineProvider();
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
					AddView(_mainView);
				}
			}
		}

		public ShapeView OverlayShapeView { get; set; }

		public Shape ClipShape {
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
			if(changed)
				_mainView?.Layout(l, t, r, b);
		}

		void ApplyShape()
		{
			this.ClipToOutline = clipShape != null;
			var provider = OutlineProvider as CometShapeOutlineProvider;
			provider.Shape = clipShape;
			this.InvalidateOutline();
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);
			this.InvalidateOutline();
		}


		class CometShapeOutlineProvider : ViewOutlineProvider
		{
			public Shape Shape { get; set; }
			public override void GetOutline(AView view, Outline outline)
			{
				if (Shape == null)
					return;
				float scale = 1;// view.Resources.DisplayMetrics.Density;
				double width = (double)view.Width * scale;
				double height = (double)view.Height* scale;
				float minDimension = (float)Math.Min(height, width);

				var bounds = new RectangleF(0, 0, view.Width, view.Height);
				var path = Shape.PathForBounds(bounds);
				//outline.
				float radius = minDimension / 2f;
				Rect rect = new Rect(0, 0, (int)width, (int)height);
				//outline.SetRoundRect(rect, radius);
				outline.SetConvexPath(path.AsAndroidPath());
			}
		}
	}

}
