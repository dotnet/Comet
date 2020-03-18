using System;
using System.Drawing;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Comet.Androids;
using AView = Android.Views.View;
using APath = Android.Graphics.Path;

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
			var provider = OutlineProvider as CometShapeOutlineProvider;
			provider.Shape = clipShape;
			this.ClipToOutline = clipShape != null;
			this.InvalidateOutline();
		}


		class CometShapeOutlineProvider : ViewOutlineProvider
		{
			public Shape Shape { get; set; }
			RectangleF lastBounds;
			APath currentPath;
			public override void GetOutline(AView view, Outline outline)
			{
				if (Shape == null)
					return;
				var bounds = new RectangleF(0, 0, view.Width, view.Height);
				if (bounds != lastBounds)
				{
					var path = Shape.PathForBounds(bounds);
					currentPath = path.AsAndroidPath();
					lastBounds = bounds;
				}
				//outline.SetRoundRect(rect, radius);
				outline.SetConvexPath(currentPath);
			}
		}
	}

}
