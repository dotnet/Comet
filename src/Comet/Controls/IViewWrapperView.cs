using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet
{
	internal class IViewWrapperView : View, IView, IReplaceableView
	{
		public IView View { get; set; }
		public IViewWrapperView(IView view) => View = view;


		public override void LayoutSubviews(Rect frame)
		{
			this.Frame = frame;
			View?.Arrange(frame);
		}
		public override Rect Frame
		{
			get => base.Frame;
			set
			{
				base.Frame = value;
				View?.Handler?.PlatformArrange(value);
			}
		}
		public override Size GetDesiredSize(Size availableSize)
		{
			if (View != null)
			{
				return View.Measure(availableSize.Width, availableSize.Height);
			}

			return base.GetDesiredSize(availableSize);
		}

		IView IReplaceableView.ReplacedView => View ?? this;

		string IView.AutomationId => View.AutomationId;

		FlowDirection IView.FlowDirection => View.FlowDirection;

		LayoutAlignment IView.HorizontalLayoutAlignment => View.HorizontalLayoutAlignment;

		LayoutAlignment IView.VerticalLayoutAlignment => View.VerticalLayoutAlignment;

		Semantics IView.Semantics => View.Semantics;

		IShape IView.Clip => View.Clip;

		IShadow IView.Shadow => View.Shadow;

		bool IView.IsEnabled => View.IsEnabled;

		bool IView.IsFocused { get => View.IsFocused; set => View.IsFocused = value; }

		Visibility IView.Visibility => View.Visibility;

		double IView.Opacity => View.Opacity;

		Paint IView.Background => View.Background;


		double IView.Width => View.Width;

		double IView.MinimumWidth => View.MinimumWidth;

		double IView.MaximumWidth => View.MaximumWidth;

		double IView.Height => View.Height;

		double IView.MinimumHeight => View.MinimumHeight;

		double IView.MaximumHeight => View.MaximumHeight;

		Thickness IView.Margin => View.Margin;

		Size IView.DesiredSize => View.DesiredSize;

		int IView.ZIndex => View.ZIndex;

		IViewHandler IView.Handler { get => View.Handler; set => View.Handler = value; }
		IElementHandler IElement.Handler { get => View.Handler; set => ((IElement)View).Handler = value; }

		bool IView.InputTransparent => View.InputTransparent;

		double ITransform.TranslationX => View.TranslationX;

		double ITransform.TranslationY => View.TranslationY;

		double ITransform.Scale => View.Scale;

		double ITransform.ScaleX => View.ScaleX;

		double ITransform.ScaleY => View.ScaleY;

		double ITransform.Rotation => View.Rotation;

		double ITransform.RotationX => View.RotationX;

		double ITransform.RotationY => View.RotationY;

		double ITransform.AnchorX => View.AnchorX;

		double ITransform.AnchorY => View.AnchorY;

		bool IView.Focus() => View.Focus();
		void IView.Unfocus() => View.Unfocus();
	}
}
